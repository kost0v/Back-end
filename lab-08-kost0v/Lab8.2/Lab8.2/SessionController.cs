using Microsoft.AspNetCore.Mvc;

namespace Lab8._2
{
    public class SessionController : Controller
    {
        public IActionResult SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
            return Content($"Session set: {key} = {value}");
        }
        public IActionResult GetSession(string key)
        {
            var value = HttpContext.Session.GetString(key);
            return Content(value ?? "Session not found");
        }
        public IActionResult SetCookie(string key, string value)
        {
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30)
            };
            Response.Cookies.Append(key, value, option);
            return Content($"Cookie set: {key} = {value}");
        }
        public IActionResult GetCookie(string key)
        {
            var value = Request.Cookies[key];
            return Content(value ?? "Cookie not found");
        }
        public IActionResult Index()
        {
            return View();
        }
    }

}
