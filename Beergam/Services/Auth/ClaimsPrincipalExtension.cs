using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Beergam.Services.Auth;

public static class ClaimsPrincipalExtensions
{
    public static string GetPin(this ClaimsPrincipal user)
        => user.FindFirstValue(JwtRegisteredClaimNames.Sub)
           ?? throw new InvalidOperationException("Pin não encontrado no token.");
}