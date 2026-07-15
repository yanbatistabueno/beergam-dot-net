namespace Beergam.Services.Auth;
using Microsoft.Extensions.Options;
public class Cookies : ICookies
{
    private readonly JwtSettings _settings;
    public Cookies(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
    }
    
    public CookieOptions GetTokenCookieOptions()
    {
        return new CookieOptions
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            Secure = false,
            Expires = DateTimeOffset.UtcNow.AddMinutes(_settings.ExpirationMinutes)
        };
    }
    
    public CookieOptions GetRefreshTokenCookieOptions()
    {
        return new CookieOptions
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            Secure = false,
            Expires = DateTimeOffset.UtcNow.AddDays(_settings.RefreshTokenExpirationDays)
        };
    }
}