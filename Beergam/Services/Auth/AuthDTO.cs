namespace Beergam.Services.Auth;
using Services.User;
public class AuthDTO
{
    public record LoginRequestDto(string Email, string Password);
    public record LoginResponseDto(UserDTO.UserDto User);
    public record RegisterRequestDto(string Name, string Email, string Password);
    public record RegisterResponseDto(UserDTO.UserDto User);
}