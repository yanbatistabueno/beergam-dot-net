namespace Beergam.Services.Cache;

public interface ICacheService
{
    public Task<T?> GetAsync<T>(string key);
    public Task SetAsync<T>(string key, T value,  TimeSpan? expiration = null);
    public Task RemoveAsync(string key);
    public Task<TimeSpan?> GetTtlAsync(string key);
}