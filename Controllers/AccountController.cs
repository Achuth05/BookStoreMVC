using BookStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace BookStoreMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly BookStoreContext _context;

        public AccountController(BookStoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault(u =>
                u.Email == model.Email &&
                u.Password == model.Password);

            if (user == null)
            {
                ViewBag.Error = "Invalid Email or Password";
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("Role", user.Role);

            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}