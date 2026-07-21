using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
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

    private async Task<(string token, string refreshToken)> IssueTokens(UserDTO.UserDto user)
    {
        var (token, jti) = _jwtService.GenerateToken(user);
        var refreshToken = _jwtService.GenerateRefreshToken();
        var ttl = await _authCache.GetTokenExpiration(user.Pin) ?? TimeSpan.FromDays(_settings.RefreshTokenExpirationDays);
        await _authCache.SetCacheUserRefreshToken(user.Pin, refreshToken, ttl);
        await _authCache.SetCacheUserCurrentJti(user.Pin, jti, TimeSpan.FromMinutes(_settings.ExpirationMinutes));
        return (token, refreshToken);
    }

    public async Task<(string token, string refreshToken)> RefreshToken(string accessToken, string refreshToken)
    {
        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        var pin = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var revocation = await _authCache.GetRevocationToken(pin);
        if (refreshToken == revocation?.RevokedToken)
        {
            throw new Exception("Refresh token invalid.");
        }
        var currentToken = await _authCache.GetCacheUserRefreshToken(pin);
        if (refreshToken == currentToken)
        {
            return await IssueTokens(await _userService.GetUserByPin(pin)); 
        }
        throw new Exception("Refresh token invalid.");
        
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
        var prev = await _authCache.GetCacheUserRefreshToken(user.Pin);
        if (prev != null)
        {
            await _authCache.SetRevocationToken(user.Pin, prev, RevocationReason.Login, TimeSpan.FromDays(_settings.RefreshTokenExpirationDays));
        }
        
        var (token, refreshToken) = await IssueTokens(user);
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

            var createdUser = new UserModel
            {
                Name = request.Name,
                Email = request.Email,
                Password = _passwordService.HashPassword(request.Password),
                IsActive = true,
                Pin = _userService.GenerateUserPin(),
                Role = UserRole.Master
            };
            var user = await _userService.CreateUser(createdUser); 
            var (token, refreshToken) = await IssueTokens(user);
            return (user, token, refreshToken);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }
}