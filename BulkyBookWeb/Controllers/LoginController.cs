using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Signup obj)
        {
            if (!ModelState.IsValid)
            {
                var user = new User { UserName = obj.Name, Email = obj.Email };
                
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Register", "Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Login obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Username == "admin" && obj.Password == "password")
                {
                    // Creating the secuirity context:
                    var claims = new List<Claim>{
                        new Claim(ClaimTypes.Name, obj.Username),
                        new Claim(ClaimTypes.Email, "email@email.com"),
                        new Claim("Department", "admin"),
                        new Claim("EmploymentDate", "2022-04-15")
                    };
                    var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = obj.RememberMe
                    };
                    
                    await HttpContext.SignInAsync("MyCookieAuth", principal, authProperties);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
