using BookStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreMVC.Filters;

namespace BookStoreMVC.Controllers
{
    [RoleAuthorize("Customer")]
    public class OrderController : Controller
    {
        private readonly BookStoreContext _context;

        public OrderController(BookStoreContext context)
        {
            _context = context;
        }

        public IActionResult MyOrders()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                                 .Where(o => o.UserId == userId)
                                 .OrderByDescending(o => o.OrderDate)
                                 .ToList();

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
            return View(orders);
        }
        public IActionResult Details(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var order = _context.Orders
                                .Include(o => o.OrderItems!)
                                .ThenInclude(oi => oi.Book)
                                .FirstOrDefault(o => o.OrderId == id &&
                                                    o.UserId == userId);

            if (order == null)
                return NotFound();

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
            return View(order);
        }
    }
}