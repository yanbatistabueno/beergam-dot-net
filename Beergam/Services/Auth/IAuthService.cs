namespace Beergam.Services.Auth;
using Beergam.Services.User;
public interface IAuthService
{
    Task<AuthDTO.LoginResponseDto> Login(AuthDTO.LoginRequestDto request);
    Task<AuthDTO.RegisterResponseDto> Register(AuthDTO.RegisterRequestDto request);
}