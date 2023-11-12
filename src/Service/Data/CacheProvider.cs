using Microsoft.Extensions.Caching.Memory;
using Frequency.Common.Entity;

namespace Frequency.Data;

public class CacheProvider : ICacheProvider {
    private static readonly SemaphoreSlim CategorySem = new(1, 1);
    private readonly IMemoryCache _cache;
    public CacheProvider(IMemoryCache memoryCache) => _cache = memoryCache;


    public async Task<IEnumerable<Category>?> GetCachedCategory() {
        try {
            return await GetCachedResponse(CacheKey.Category, CategorySem);
        }
        catch {
            throw;
        }
    }

    private async Task<IEnumerable<Category>?> GetCachedResponse(string cacheKey, SemaphoreSlim sem) {
        var isAvailable = _cache.TryGetValue(cacheKey, out List<Category>? categories);
        if (isAvailable) {
            return categories;
        }

        try {
            await sem.WaitAsync();
            isAvailable = _cache.TryGetValue(cacheKey, out categories);
            if (isAvailable) {
                return categories;
            }

            /* TODO: Cache sample responses
            categories = categoriesService.GetCategoriesDetailsFromDB();
            var cacheEntryOptions = new MemoryCacheEntryOptions {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Size = 1024,
            };
            _cache.Set(cacheKey, categories, cacheEntryOptions);
            */
        }
        catch {
            throw;
        }
        finally {
            sem.Release();
        }

        return categories;
    }
}