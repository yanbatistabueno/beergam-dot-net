using Beergam.Services.User;

namespace Beergam.Services.Auth;
using Beergam.Services.Cache;
public class AuthCache : IAuthCache
{
    private readonly ICacheService _cacheService;
    private static string KeyAuthPrefix = "AUTH:";
    private static string KeyTokenPrefix = "ACCESS_TOKEN:";
    private static string KeyInvalidatePrefix = $"{KeyAuthPrefix}{KeyTokenPrefix}INVALIDATE:";
    public AuthCache(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    
    public Task SetCacheUserRefreshToken(string pin, string token,  TimeSpan? expiration = null)
    {
        var key = $"{KeyAuthPrefix}{KeyTokenPrefix}{pin}";
        return _cacheService.SetAsync(key, token, expiration);
    }

    public Task SetInvalidateCacheUserRefreshToken(string pin, string token)
    {
        var key = $"{KeyInvalidatePrefix}{pin}";
        return _cacheService.SetAsync(key, token);
    }
    public Task GetInvalidateCacheUserRefreshToken(string pin, string token)
    {
        var key = $"{KeyInvalidatePrefix}{pin}";
        return _cacheService.GetAsync<string>(key);
    }
}