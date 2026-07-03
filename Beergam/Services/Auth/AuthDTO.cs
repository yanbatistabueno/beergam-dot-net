namespace Beergam.Services.Auth;
using Services.User;
public class AuthDto
{
    public record LoginRequestDTO(string Email, string Password);

    public record LoginResponseDTO(UserDto User, string Token);
}