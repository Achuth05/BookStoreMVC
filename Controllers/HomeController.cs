using Microsoft.AspNetCore.Mvc;
using BookStoreMVC.Models;

namespace BookStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookStoreContext _context;

        public HomeController(BookStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var featuredBooks = _context.Books
                                        .OrderByDescending(b => b.BookId)
                                        .Take(4)
                                        .ToList();

            return View(featuredBooks);
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