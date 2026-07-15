namespace Beergam.Services.Auth;
using Beergam.Services.User;
public interface IAuthService
{
    Task<(UserDTO.UserDto user, string token, string refreshToken)> Login(AuthDTO.LoginRequestDto request);
    Task<(UserDTO.UserDto user, string token, string refreshToken)> Register(AuthDTO.RegisterRequestDto request);
}