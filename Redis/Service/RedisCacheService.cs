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
            try
            {

                string? cachedData = await _cache.GetStringAsync(cacheKey);

                if (string.IsNullOrEmpty(cachedData))
                {
                    return default;
                }

                return JsonSerializer.Deserialize<T>(cachedData);

            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex, "Redis is offline or timed out. Returning null to force DB fallback.");

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
                _logger.LogWarning(ex, "Redis is offline or timed out. Returning null to force DB fallback.");
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
                _logger.LogWarning(ex, "Redis is offline or timed out. Returning null to force DB fallback.");
            }


        }
    }
}
