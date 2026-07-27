using BookStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly BookStoreContext _context;

        public BookController(BookStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var books = _context.Books
                                .Include(b => b.Category)
                                .ToList();

            return View(books);
        }

        public IActionResult Categories()
        {
            return View();
        }

        public IActionResult Offers()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books
                            .Include(b => b.Category)
                            .FirstOrDefault(b => b.BookId == id);

            if (book == null)
                return NotFound();

            return View(book);
        }
        [HttpPost]
        public IActionResult AddToCart(int bookId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var existingItem = _context.Cart
                                    .FirstOrDefault(c =>
                                        c.UserId == userId &&
                                        c.BookId == bookId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                Cart cart = new Cart
                {
                    UserId = userId.Value,
                    BookId = bookId,
                    Quantity = 1
                };

                _context.Cart.Add(cart);
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Cart");
        }
    }
}