using Beergam.Services.Auth;

namespace Beergam.Services.User;
using Beergam.Services.Password;
using Beergam.Data;
using Microsoft.EntityFrameworkCore;
public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> VerifyEmailExists(string email)
    {
        var loweredEmail = email.Trim().ToLower();
        return await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == loweredEmail);
    }
    public async Task<UserDTO.UserDto> CreateUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return new UserDTO.UserDto(user.Name, user.Pin, user.Email);
    }

}