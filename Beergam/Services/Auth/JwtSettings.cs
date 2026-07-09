namespace Beergam.Services.Auth;

public class JwtSettings
{
    public string Issuer { get; } = string.Empty;
    public string Audience { get; } = string.Empty;
    public string SecretKey { get; } = string.Empty;
    public int ExpirationMinutes { get; } = 10;
}