namespace Beergam.Services.Cache;
using StackExchange.Redis;
using System.Text.Json;
public class CacheService : ICacheService
{
    private readonly IDatabase _db;
    private static readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(5);
    public CacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await _db.StringGetAsync(key);
            return value.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>((string)value!);
            
        }
        catch
        {
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiration ?? _defaultExpiration);
        }
        catch (RedisException ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _db.KeyDeleteAsync(key);
        }
        catch (RedisException ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async Task<TimeSpan?> GetTtlAsync(string key)
    {
        try
        {
            var ttl = await _db.KeyTimeToLiveAsync(key);
            return ttl;
        }
        catch (RedisException ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}