using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TraineeManagement.api.Redis.Repository;

namespace TraineeManagement.api.Redis.Service
{
    public class RedisCacheService : IRedisCacheRepo
    {
        
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
        {
            _cache = cache;

            _logger = logger;
        }

        public async Task<T?> GetItem<T>(string cacheKey)
        {
            string? cachedData;
            try
            {
                cachedData = await _cache.GetStringAsync(cacheKey);
                if (string.IsNullOrEmpty(cachedData))
                {
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis is offline or timed out while getting key '{CacheKey}'. Falling back to database.", cacheKey);
                return default;
            }

            // Deserializing OUTSIDE the try-catch for Redis, 
            // so a bad JSON payload doesn't falsely report a Redis outage.
            try
            {
                return JsonSerializer.Deserialize<T>(cachedData);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "Failed to deserialize cache data for key '{CacheKey}'.", cacheKey);
                return default;
            }

        }

        public async Task RemoveItem(string key)
        {

            try
            {
                await _cache.RemoveAsync(key);

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis is offline or timed out while removing key '{Key}'.", key);
            }

        }

        public async Task SetItem<T>(string cacheKey, T item)
        {
            try
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    //SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                string serializedData = JsonSerializer.Serialize(item);

                await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis is offline or timed out while setting key '{CacheKey}'. Data was not cached.", cacheKey);
            }


        }
    }
}
