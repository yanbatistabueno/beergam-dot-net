namespace Beergam.Services.User;
using Beergam.Services.Password;
using Beergam.Data;
using Microsoft.EntityFrameworkCore;
public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordService _passwordService;

    public UserService(AppDbContext dbContext, IPasswordService passwordService)
    {
        _dbContext = dbContext;
        _passwordService = passwordService;
    }
    public async Task<bool> VerifyEmailExists(string email)
    {
        var loweredEmail = email.Trim().ToLower();
        return await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == loweredEmail);
    }
    public async Task<UserDTO.UserDto> RegisterUserAsync(User user)
    {
        if(await VerifyEmailExists(user.Email))
        {
            throw new Exception("Email already exists");
        }
        var CreatedUser = new User
        {
            Name = user.Name,
            Email = user.Email,
            Password = _passwordService.HashPassword(user.Pin),
            IsActive = true,
            Pin = "1234",
            MasterPin = "1234",
            Role = UserRole.Master
        };
        _dbContext.Users.Add(CreatedUser);
        await _dbContext.SaveChangesAsync();
        return new UserDTO.UserDto(CreatedUser.Name, CreatedUser.Pin, CreatedUser.Email);
    }

}