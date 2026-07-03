using Microsoft.AspNetCore.Identity;

namespace Beergam.Services.Password;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<object> _hasher = new();
    public string HashPassword(string password)
    {
        return _hasher.HashPassword(null, password);
    }
    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}