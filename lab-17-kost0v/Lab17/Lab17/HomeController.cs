using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;
using Lab17;
namespace Lab17.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        ExampleService exservice;
        public HomeController(IMemoryCache cache, Labcontext _context,
       IDistributedCache distributedCache)
        {
            exservice = new ExampleService(cache, _context, distributedCache);
        }
        [HttpGet("Database")]
        public IActionResult GetExamplesDatabase()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var examples = exservice.GetExamplesDatabase();
            sw.Stop();
            ViewData["AllTable"] = examples;
            ViewData["AllTime"] = sw.ElapsedMilliseconds;
            return View("Index");
        }
        [HttpGet("MemoryCache")]
        public IActionResult GetExamplesMemoryCache()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var examples = exservice.GetExamplesMemoryCache();
            sw.Stop();
            ViewData["AllTablecache"] = examples;
            ViewData["AllTimecache"] = sw.ElapsedMilliseconds;
            return View("Index");
        }
        [HttpGet("DistributedCache")]
        public async Task<IActionResult> GetExamplesDistributedCache()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var examples = await exservice.GetExamplesDistributedCacheAsync();
            sw.Stop();
            ViewData["AllTablecache"] = examples;
            ViewData["AllTimecache"] = sw.ElapsedMilliseconds;
            return View("Index");
        }
    }
}
