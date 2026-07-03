namespace Beergam.Services.User;

public class UserDTO
{
    public record UserDTO(string Name, string Pin, string Email);
}