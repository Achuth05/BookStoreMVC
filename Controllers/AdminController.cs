using BookStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.EntityFrameworkCore;
using BookStoreMVC.Filters;

namespace BookStoreMVC.Controllers
{
    [RoleAuthorize("Admin")]
    public class AdminController : Controller
    {
        private readonly BookStoreContext _context;

        public AdminController(BookStoreContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalBooks = _context.Books.Count();
            ViewBag.TotalCategories = _context.Categories.Count();
            ViewBag.TotalCustomers = _context.Users.Count(u => u.Role == "Customer");
            ViewBag.TotalOrders = _context.Orders.Count();
            ViewBag.PendingOrders = _context.Orders.Count(o => o.Status == "Pending");
            ViewBag.DeliveredOrders = _context.Orders.Count(o => o.Status == "Delivered");
            ViewBag.TotalRevenue = _context.Orders
                                        .Where(o => o.Status == "Delivered")
                                        .Sum(o => (decimal?)o.TotalAmount) ?? 0;

            return View();
        }

        public IActionResult Categories()
        {
            var categories = _context.Categories.ToList();

            return View(categories);
        }
        // GET
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }

        // GET
        public IActionResult EditCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // POST
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            _context.Categories.Update(category);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("Categories");
        }

        public IActionResult Books()
        {
            var books = _context.Books.ToList();

            return View("Books",books);
        }
        public IActionResult CreateBook()
        {
            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            ViewBag.Images = Directory.GetFiles(imageFolder)
                                    .Select(Path.GetFileName)
                                    .ToList();

            return View();
        }
        [HttpPost]
        public IActionResult CreateBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();

                var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                ViewBag.Images = Directory.GetFiles(imageFolder)
                                        .Select(Path.GetFileName)
                                        .ToList();

                return View(book);
            }

            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("Books");
        }
        // GET
        public IActionResult EditBook(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
                return NotFound();

            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            var imageFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "images");

            ViewBag.Images = Directory.GetFiles(imageFolder)
                                    .Select(Path.GetFileName)
                                    .ToList();

            return View(book);
        }
        [HttpPost]
        public IActionResult EditBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();

                var imageFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images");

                ViewBag.Images = Directory.GetFiles(imageFolder)
                                        .Select(Path.GetFileName)
                                        .ToList();

                return View(book);
            }

            _context.Books.Update(book);
            _context.SaveChanges();

            return RedirectToAction("Books");
        }
        public IActionResult DeleteBook(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();

            return RedirectToAction("Books");
        }
        public IActionResult ManageOrders()
        {
            var orders = _context.Orders
                                .Include(o => o.User)
                                .OrderByDescending(o => o.OrderDate)
                                .ToList();

            return View(orders);
        }
        public IActionResult EditOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
        [HttpPost]
        public IActionResult EditOrder(Order model)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == model.OrderId);

            if (order == null)
                return NotFound();

            order.Status = model.Status;

            _context.SaveChanges();

            return RedirectToAction("ManageOrders");
        }
    }
}