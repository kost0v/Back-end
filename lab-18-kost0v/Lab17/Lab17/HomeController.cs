using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using Prometheus;
using System.Diagnostics;
using Lab17;
using System.Diagnostics.Metrics;
namespace Lab18.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private static readonly Counter TotalRequests =
       Metrics.CreateCounter("total_requests", "Total number of requests");
        private static readonly Gauge MemoryUsage =
       Metrics.CreateGauge("memory_usage_bytes", "Current memory usage in bytes");
        private static readonly Gauge CpuUsage =
       Metrics.CreateGauge("cpu_usage_seconds_total", "Total CPU usage in seconds");
        // Сервис для работы с кэшем ExampleService exservice;
        // Конструктор контроллера
        public HomeController(IMemoryCache cache, Labcontext _context,
       IDistributedCache distributedCache)
        {
            // Инициализация сервиса с кэшем памяти и распределенным кэшем
            exservice = new ExampleService(cache, _context, distributedCache);
        }
        // Получение всех примеров из базы данных
        [HttpGet("Database")]
        public IActionResult GetExamplesDatabase()
        {
            TotalRequests.Inc();
            MemoryUsage.Set(Process.GetCurrentProcess().WorkingSet64);
            CpuUsage.Set(Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds);
            // Измерение времени выполнения запроса
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var examples = exservice.GetExamplesDatabase();
            sw.Stop();
            // Передача данных во View
            ViewData["AllTable"] = examples;
            ViewData["AllTime"] = sw.ElapsedMilliseconds;
            return View("Index");
        }
        // Получение всех примеров из кэша памяти
        [HttpGet("MemoryCache")]
        public IActionResult GetExamplesMemoryCache()
        {
            TotalRequests.Inc();
            MemoryUsage.Set(Process.GetCurrentProcess().WorkingSet64);
            CpuUsage.Set(Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds);
            // Измерение времени выполнения запроса
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var examples = exservice.GetExamplesMemoryCache();
            sw.Stop();
            // Передача данных во View
            ViewData["AllTablecache"] = examples;
            ViewData["AllTimecache"] = sw.ElapsedMilliseconds;
            return View("Index");
        }
        // Получение всех примеров из распределенного кэша
        [HttpGet("DistributedCache")]
        public async Task<IActionResult> GetExamplesDistributedCache()
        {
            TotalRequests.Inc();
            MemoryUsage.Set(Process.GetCurrentProcess().WorkingSet64);
            CpuUsage.Set(Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds);
            // Измерение времени выполнения запроса
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var examples = await exservice.GetExamplesDistributedCacheAsync();
            sw.Stop();
            // Передача данных во View
            ViewData["AllTablecache"] = examples;
            ViewData["AllTimecache"] = sw.ElapsedMilliseconds;
            return View("Index");
        }
    }
}