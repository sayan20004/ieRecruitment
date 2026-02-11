using Microsoft.AspNetCore.Mvc;
using ieRecruitment.Models;

namespace ieRecruitment.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Implement authentication logic
                // For now, just redirect to home
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Implement user registration logic
                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            // TODO: Implement logout logic
            return RedirectToAction("Login");
        }
    }
}
