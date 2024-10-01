using Lab17;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Lab17
{
    public class ExampleService
    {
        // кэш памяти
        public IMemoryCache _cache;
        private Labcontext _context;
        // распределенного кэша
        private readonly IDistributedCache _distributedCache;
        // Конструктор сервиса, принимающий зависимости через DI
        public ExampleService(IMemoryCache memoryCache, Labcontext context,
       IDistributedCache distributedCache)
        {
            _cache = memoryCache; // Инициализация кэша памяти
            _context = context; // Инициализация контекста базы данных
            _distributedCache = distributedCache; // Инициализация распределенного
        }
        // Получение всех примеров из базы данных или кэша памяти
        public Example[] GetExamplesMemoryCache()
        {
            // Проверка, есть ли данные в кэше памяти
            if (!_cache.TryGetValue("examples", out Example[] examples))
            {
                // Если данных нет в кэше, извлекаем их из базы данных
                examples = _context.Examples.ToArray();
                // Кэшируем данные в памяти с абсолютным временем жизни 5 минут
                _cache.Set("examples", examples, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }
            // Возвращаем данные (из кэша или базы данных)
            return examples;
        }
        // Получение всех примеров из базы данных или распределенного кэша
        public async Task<Example[]> GetExamplesDistributedCacheAsync()
        {
            // Ключ для кэширования данных
            var cacheKey = "examples_all";
            // Получение данных из распределенного кэша
            var examplesJson = await _distributedCache.GetStringAsync(cacheKey);
            if (examplesJson != null)
            {
                // Если данные найдены в кэше, десериализуем их
                return JsonSerializer.Deserialize<Example[]>(examplesJson);
            }
            else
            {
                // Если данные не найдены в кэше, извлекаем их из базы данных
                var examples = _context.Examples.ToArray();
                // Кэшируем данные в распределенном кэше с абсолютным временем жизни 5s
                 await _distributedCache.SetStringAsync(cacheKey,
                JsonSerializer.Serialize(examples), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
                return examples;
            }
        }
        // Получение всех примеров непосредственно из базы данных
        public Example[] GetExamplesDatabase()
        {
            // Извлечение данных из базы данных без кэширования
            return _context.Examples.ToArray();
        }
    }
}