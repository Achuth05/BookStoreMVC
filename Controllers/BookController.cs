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

        public IActionResult Index(string? search, int? category, string? sort)
        {
            var books = _context.Books
                                .Include(b => b.Category)
                                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                books = books.Where(b =>
                    b.Title.Contains(search) ||
                    b.Author.Contains(search));
            }

            // Filter
            if (category.HasValue)
            {
                books = books.Where(b => b.CategoryId == category);
            }

            // Sort
            switch (sort)
            {
                case "priceAsc":
                    books = books.OrderBy(b => b.Price);
                    break;

                case "priceDesc":
                    books = books.OrderByDescending(b => b.Price);
                    break;

                case "name":
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            ViewBag.Categories = _context.Categories.ToList();

            ViewBag.Search = search;
            ViewBag.SelectedCategory = category;
            ViewBag.Sort = sort;
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
            return View(books.ToList());
        }

        public IActionResult Categories()
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