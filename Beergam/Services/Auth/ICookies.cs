namespace Beergam.Services.Auth;

public interface ICookies
{
    CookieOptions GetTokenCookieOptions();
    CookieOptions GetRefreshTokenCookieOptions();
}