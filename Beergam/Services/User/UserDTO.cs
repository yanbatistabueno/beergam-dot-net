namespace Beergam.Services.User;

public class UserDTO
{
    public record UserDto(string Name, string Pin, string Email);
}