using BookStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly BookStoreContext _context;

        public CartController(BookStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cartItems = _context.Cart
                                    .Include(c => c.Book)
                                    .Where(c => c.UserId == userId)
                                    .ToList();

            return View(cartItems);
        }
        public IActionResult Remove(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var item = _context.Cart.FirstOrDefault(c =>
                            c.CartId == id &&
                            c.UserId == userId);

            if (item != null)
            {
                _context.Cart.Remove(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public IActionResult Increase(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var item = _context.Cart.FirstOrDefault(c =>
                c.CartId == id &&
                c.UserId == userId);

            if (item != null)
            {
                item.Quantity++;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public IActionResult Decrease(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var item = _context.Cart.FirstOrDefault(c =>
                c.CartId == id &&
                c.UserId == userId);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    _context.Cart.Remove(item);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Checkout()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cartItems = _context.Cart
                                    .Include(c => c.Book)
                                    .Where(c => c.UserId == userId)
                                    .ToList();

            if (!cartItems.Any())
                return RedirectToAction("Index");

            decimal total = cartItems.Sum(c => c.Quantity * c.Book!.Price);

            Order order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Insert into OrderItems
            foreach (var item in cartItems)
            {
                OrderItem orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Book!.Price
                };

                _context.OrderItems.Add(orderItem);
            }
            // Reduce stock
            foreach (var item in cartItems)
            {
                var book = _context.Books.FirstOrDefault(b => b.BookId == item.BookId);

                if (book != null)
                {
                    if (book.Stock < item.Quantity)
                    {
                        return Content($"Not enough stock available for {book.Title}");
                    }

                    book.Stock -= item.Quantity;
                }
            }
            _context.Cart.RemoveRange(cartItems);
            _context.SaveChanges();

            return RedirectToAction("Success");
        }
        public IActionResult Success()
        {
            return View();
        }
    }
}