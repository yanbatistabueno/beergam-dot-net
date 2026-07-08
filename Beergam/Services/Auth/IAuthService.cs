namespace Beergam.Services.Auth;
using Beergam.Services.User;
public interface IAuthService
{
    Task<AuthDTO.LoginResponseDto> Login(AuthDTO.LoginRequestDto request);
    Task<UserDTO.UserDto> Register(AuthDTO.RegisterRequestDto request);
}