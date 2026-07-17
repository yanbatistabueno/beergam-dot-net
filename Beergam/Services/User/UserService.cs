using Beergam.Services.Auth;
using System.Security.Cryptography;
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

    private string GenerateRandomPin()
    {
        const string Symbols = "!@#$%^&*()-_=+[]{};:,.?";
        char[] Chars =
        (
            new string(Enumerable.Range('A', 26).Select(c => (char)c).ToArray()) +   // A–Z
            new string(Enumerable.Range('a', 26).Select(c => (char)c).ToArray()) +   // a–z
            "0123456789" +
            Symbols
        ).ToCharArray();
        char[] result = new char[6];
        for (int i = 0; i < 6; i++)
        {
            // Use RandomNumberGenerator for true cryptographic randomness
            result[i] = Chars[RandomNumberGenerator.GetInt32(Chars.Length)];
        }
        return "BG_" + new string(result);
    }

    public string GenerateUserPin()
    {
        string pin;
        do
        {
            pin = GenerateRandomPin();
        }
        while (_dbContext.Users.Any(u => u.Pin == pin));
        return pin;
    }

    public async Task<bool> VerifyEmailExists(string email)
    {
        String loweredEmail = email.Trim().ToLower();
        return await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == loweredEmail);
    }
    public async Task<UserDTO.UserDto> CreateUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return new UserDTO.UserDto(user.Name, user.Pin, user.Email);
    }

    public async Task<UserDTO.UserDto> GetUserByEmail(string email)
    {
        String loweredEmail = email.Trim().ToLower();
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loweredEmail);
        if (user is null)
        {
            throw new Exception("Usuário não encontrado.");
        }
        return new UserDTO.UserDto(user.Name, user.Pin, user.Email);
    }
    
    public async Task<UserDTO.UserDto> GetUserByPin(string pin)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Pin == pin);
        if (user is null)
        {
            throw new Exception("Usuário não encontrado.");
        }
        return new UserDTO.UserDto(user.Name, user.Pin, user.Email);
    }
    
    public async Task<bool> VerifyPassword(string email, string password)
    {
        String loweredEmail = email.Trim().ToLower();

        var hashedPassword = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.Email.ToLower() == loweredEmail)
            .Select(u => u.Password)
            .FirstOrDefaultAsync();
        if (hashedPassword is null)
            return false;

        return _passwordService.VerifyHashedPassword(hashedPassword, password);
    }
}