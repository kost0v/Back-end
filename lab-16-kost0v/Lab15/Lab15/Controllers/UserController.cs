using Microsoft.AspNetCore.Mvc;
namespace Lab15.Controlles
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
