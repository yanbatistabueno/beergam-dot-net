using Beergam.Services.User;

namespace Beergam.Services.Auth;
using Beergam.Services.Cache;
public class AuthCache : IAuthCache
{
    private readonly ICacheService _cacheService;
    private static string KeyAuthPrefix = "AUTH:";
    private static string KeyTokenPrefix = $"{KeyAuthPrefix}REFRESH_TOKEN:";
    private static string KeyInvalidatePrefix = $"{KeyTokenPrefix}REVOCATION:";
    private static string KeyJtiPrefix = $"{KeyAuthPrefix}JTI:";
    public AuthCache(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public Task<string?> GetCacheUserCurrentJti(string pin)
    {
        var key = $"{KeyJtiPrefix}{pin}";
        return _cacheService.GetAsync<string?>(key);
    }

    public Task SetCacheUserCurrentJti(string pin, string token,  TimeSpan? expiration = null)
    {
        var key = $"{KeyJtiPrefix}{pin}";
        return _cacheService.SetAsync(key, token, expiration);
    }

    public Task<string?> GetCacheUserRefreshToken(string pin)
    {
        var key = $"{KeyTokenPrefix}{pin}";
        return _cacheService.GetAsync<string?>(key);
    }
    
    public Task SetCacheUserRefreshToken(string pin, string token,  TimeSpan? expiration = null)
    {
        var key = $"{KeyTokenPrefix}{pin}";
        return _cacheService.SetAsync(key, token, expiration);
    }

    public Task SetRevocationToken(string pin, string token, RevocationReason reason, TimeSpan? expiration = null)
    {
        var revocation = new RevocationInfo(token, reason, DateTime.UtcNow, ""); // You might want to replace "" with the actual IP
        var key = $"{KeyInvalidatePrefix}{pin}";
        return _cacheService.SetAsync(key, revocation, expiration);
    }
    public Task<RevocationInfo?> GetRevocationToken(string pin)
    {
        var key = $"{KeyInvalidatePrefix}{pin}";
        return _cacheService.GetAsync<RevocationInfo?>(key);
    }

    public Task<TimeSpan?> GetTokenExpiration(string pin)
    {
        var key = $"{KeyTokenPrefix}{pin}";
        return _cacheService.GetTtlAsync(key);
    }
}