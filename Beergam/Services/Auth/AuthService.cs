using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;

namespace Beergam.Services.Auth;
using Beergam.Services.User;
using Beergam.Services.Password;
public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _settings;
    private readonly IAuthCache _authCache;

    public AuthService(IUserService userService, IPasswordService passwordService, IJwtService jwtService, IOptions<JwtSettings> settings, IAuthCache authCache)
    {
        _userService = userService;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _settings = settings.Value;
        _authCache = authCache;
    }
    
    public async Task<(UserDTO.UserDto user, string token, string refreshToken)> Login(AuthDTO.LoginRequestDto request)
    {
        if (!await _userService.VerifyEmailExists(request.Email))
        {
            throw new Exception("Email não encontrado.");
        }
        if (!await _userService.VerifyPassword(request.Email, request.Password))
        {
            throw new Exception("Credenciais incorretas.");
        }
        var user = await _userService.GetUserByEmail(request.Email);
        var token = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        await _authCache.SetCacheUserRefreshToken(user.Pin, refreshToken, TimeSpan.FromDays(_settings.RefreshTokenExpirationDays));
        return (user, token, refreshToken);
    }

    public async Task<(UserDTO.UserDto user, string token, string refreshToken)> Register(AuthDTO.RegisterRequestDto request)
    {
        try
        {
            if (await _userService.VerifyEmailExists(request.Email))
            {
                throw new Exception("Email já está em uso.");
            }

            var createdUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = _passwordService.HashPassword(request.Password),
                IsActive = true,
                Pin = _userService.GenerateUserPin(),
                Role = UserRole.Master
            };
            var user = await _userService.CreateUser(createdUser); 
            var token =  _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            await _authCache.SetCacheUserRefreshToken(user.Pin, refreshToken, TimeSpan.FromDays(_settings.RefreshTokenExpirationDays));
            return (user, token, refreshToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }
}