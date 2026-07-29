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
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                ViewBag.CartCount = _context.Cart
                                            .Where(c => c.UserId == userId)
                                            .Sum(c => c.Quantity);
            }
            else
            {
                ViewBag.CartCount = 0;
            }
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

    }
}