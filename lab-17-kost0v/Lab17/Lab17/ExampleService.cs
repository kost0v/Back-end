using Lab17;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Lab17
{
    public class ExampleService
    {
        public IMemoryCache _cache;
        private Labcontext _context;
        private readonly IDistributedCache _distributedCache;
        public ExampleService(IMemoryCache memoryCache, Labcontext context,
       IDistributedCache distributedCache)
        {
            _cache = memoryCache; 
            _context = context; 
            _distributedCache = distributedCache; 
        }
        public Example[] GetExamplesMemoryCache()
        {
            if (!_cache.TryGetValue("examples", out Example[] examples))
            {
                examples = _context.Examples.ToArray();
                _cache.Set("examples", examples, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
            return examples;
        }
        public async Task<Example[]> GetExamplesDistributedCacheAsync()
        {
            var cacheKey = "examples_all";
            var examplesJson = await _distributedCache.GetStringAsync(cacheKey);
            if (examplesJson != null)
            {
                return JsonSerializer.Deserialize<Example[]>(examplesJson);
            }
            else
            {
                var examples = _context.Examples.ToArray();
                 await _distributedCache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(examples), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
                return examples;
            }
        }
        public Example[] GetExamplesDatabase()
        {
            return _context.Examples.ToArray();
        }
    }
}