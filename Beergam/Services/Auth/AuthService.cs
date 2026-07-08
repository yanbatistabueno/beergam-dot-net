using Microsoft.AspNetCore.Identity.Data;

namespace Beergam.Services.Auth;
using Beergam.Services.User;
using Beergam.Services.Password;
public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public AuthService(IUserService userService, IPasswordService passwordService, IJwtService jwtService)
    {
        _userService = userService;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<AuthDTO.LoginResponseDto> Login(AuthDTO.LoginRequestDto request)
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
        return new AuthDTO.LoginResponseDto(user, token);
    }

    public async Task<UserDTO.UserDto> Register(AuthDTO.RegisterRequestDto request)
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
                Pin = "12345",
                MasterPin = "12345",
                Role = UserRole.Master
            };
            UserDTO.UserDto user = await _userService.CreateUser(createdUser); 
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }
}