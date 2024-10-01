using Microsoft.AspNetCore.Mvc;

namespace Lab9
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GenerateError()
        {
            int x = 0;
            int y = 1 / x; 
            return View();
        }
        public IActionResult GenerateServerError()
        {
            throw new System.Exception("Сервер упал");
        }
    }
}
