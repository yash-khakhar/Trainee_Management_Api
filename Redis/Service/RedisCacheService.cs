using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TraineeManagement.api.Redis.Repository;

namespace TraineeManagement.api.Redis.Service
{
    public class RedisCacheService : IRedisCacheRepo
    {
        
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetItem<T>(string cacheKey)
        {
            string? cachedData = await _cache.GetStringAsync(cacheKey);

            if (string.IsNullOrEmpty(cachedData))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(cachedData);

        }

        public async Task RemoveItem(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task SetItem<T>(string cacheKey, T item)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                //SlidingExpiration = TimeSpan.FromMinutes(2)
            };

            string serializedData = JsonSerializer.Serialize(item);

            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);
        }
    }
}
