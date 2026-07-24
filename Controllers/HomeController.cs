using Microsoft.AspNetCore.Mvc;

namespace BookStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Test()
        {
            return Content("Test is working");
        }
    }
}