using Beergam.Services.User;
using System.Security.Claims;
namespace Beergam.Services.Auth;

public interface IJwtService
{
    (string token, string jti) GenerateToken(UserDTO.UserDto user);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}