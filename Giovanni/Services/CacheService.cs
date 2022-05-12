using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Giovanni.Services
{
    public class CacheService
    {
        private readonly MemoryCache _cache = new(new MemoryCacheOptions() {SizeLimit = 1024});

        public object GetOrCreate(object key, Func<object> createItem)
        {
            if (_cache.TryGetValue(key, out object cacheEntry)) return cacheEntry;

            cacheEntry = createItem();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromHours(1))
                .SetAbsoluteExpiration(TimeSpan.FromDays(1));

            _cache.Set(key, cacheEntry, cacheEntryOptions);

            return cacheEntry;
        }
        
        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createItem)
        {
            if (_cache.TryGetValue(key, out T cacheEntry))
            {
                Console.WriteLine($"Get cached {key}");                
                return cacheEntry;
            }

            Console.WriteLine($"Caching {key}");
            cacheEntry = await createItem();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1)
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromHours(1))
                .SetAbsoluteExpiration(TimeSpan.FromDays(1));

            _cache.Set(key, cacheEntry, cacheEntryOptions);

            return cacheEntry;
        }
    }
}