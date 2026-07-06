namespace Beergam.Services.User;
using Beergam.Services.Auth;
public interface IUserService
{
    Task<bool> VerifyEmailExists(string email);
    Task<UserDTO.UserDto> CreateUser(User user);
}