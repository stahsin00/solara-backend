using StackExchange.Redis;
using System.Text.Json;

// TODO: error handling (serialization/deserialization)
public class RedisCacheService
{
    private readonly IDatabase _redis;

    public RedisCacheService(IConnectionMultiplexer muxer)
    {
        _redis = muxer.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var typeKey = typeof(T).Name.ToLower();
        var value = await _redis.StringGetAsync($"{typeKey}:{key}");

        if (value.IsNullOrEmpty) throw new KeyNotFoundException($"No data found for key '{key}'.");

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var typeKey = typeof(T).Name.ToLower();
        var serializedValue = JsonSerializer.Serialize(value);

        await _redis.StringSetAsync($"{typeKey}:{key}", serializedValue, expiration);
    }

    public async Task RemoveAsync<T>(string key)
    {
        var typeKey = typeof(T).Name.ToLower();

        await _redis.KeyDeleteAsync($"{typeKey}:{key}");
    }

    public async Task<T?> GetHashAsync<T>(string key)
    {
        var typeKey = typeof(T).Name.ToLower();
        var value = await _redis.HashGetAsync(typeKey, key);

        if (value.IsNullOrEmpty) throw new KeyNotFoundException($"No data found for key '{key}'.");

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetHashAsync<T>(string key, T value)
    {
        var typeKey = typeof(T).Name.ToLower();
        var serializedValue = JsonSerializer.Serialize(value);

        await _redis.HashSetAsync(typeKey, key, serializedValue);
    }

    public async Task RemoveHashAsync<T>(string key)
    {
        var typeKey = typeof(T).Name.ToLower();

        await _redis.HashDeleteAsync(typeKey, key);
    }

    public async Task<IEnumerable<T?>> GetAllHashAsync<T>() 
    {
        var typeKey = typeof(T).Name.ToLower();

        var entries = await _redis.HashGetAllAsync(typeKey);
        var values = entries
                    .Where(entry => !entry.Value.IsNullOrEmpty)
                    .Select(entry => JsonSerializer.Deserialize<T>(entry.Value!))
                    .Where(value => value != null)
                    .ToList();

        return values;
    }
}
