namespace Beergam.Services.User;

public interface IUserService
{
    Task<bool> VerifyEmailExists(string email);
    Task<UserDTO.UserDto> RegisterUserAsync(User user);
}