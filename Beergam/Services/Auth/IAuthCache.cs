namespace Beergam.Services.Auth;

public interface IAuthCache
{
    Task SetCacheUserRefreshToken(string pin, string refreshToken, TimeSpan? expiration = null);
    Task SetInvalidateCacheUserRefreshToken(string pin, string token);
    Task GetInvalidateCacheUserRefreshToken(string pin, string token);
}