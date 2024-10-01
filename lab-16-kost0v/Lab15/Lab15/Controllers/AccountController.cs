using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lab15.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = PredefinedUsers.Users.FirstOrDefault(u => u.Username ==
           username && u.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
 {
 new Claim(ClaimTypes.Name, user.Username),
new Claim(ClaimTypes.Role, user.Role)
 };
                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.Role == "User")
                {
                    return RedirectToAction("Index", "User");
                }
            }
            ViewBag.Message = "Неверное имя пользователя или пароль.";
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
