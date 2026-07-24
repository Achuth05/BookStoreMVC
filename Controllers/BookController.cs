using Microsoft.AspNetCore.Mvc;

namespace BookStoreMVC.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Categories()
        {
            return View();
        }

        public IActionResult Offers()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}