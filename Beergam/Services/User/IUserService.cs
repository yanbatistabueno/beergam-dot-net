namespace Beergam.Services.User;
using Beergam.Services.Auth;
public interface IUserService
{
    Task<bool> VerifyEmailExists(string email);
    Task<UserDTO.UserDto> CreateUser(UserModel userModel);
    Task<UserDTO.UserDto> GetUserByEmail(string email);
    Task<bool> VerifyPassword(string email,string password);
    string GenerateUserPin();
    Task<UserDTO.UserDto> GetUserByPin(string pin);
}