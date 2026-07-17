namespace Beergam.Services.Auth;

public interface IAuthCache
{
    Task SetCacheUserRefreshToken(string pin, string refreshToken, TimeSpan? expiration = null);
    Task<string?> GetCacheUserRefreshToken(string pin);
    Task SetRevocationToken(string pin, string token, RevocationReason reason, TimeSpan? expiration = null);
    Task<RevocationInfo?> GetRevocationToken(string pin);
    Task<string?> GetCacheUserCurrentJti(string pin);
    Task SetCacheUserCurrentJti(string pin, string token, TimeSpan? expiration = null);
    Task<TimeSpan?> GetTokenExpiration(string pin);
}