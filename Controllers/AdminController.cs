using BookStoreMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly BookStoreContext _context;

        public AdminController(BookStoreContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
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
            return Content("Books Page");
        }

        public IActionResult Orders()
        {
            return Content("Orders Page");
        }
    }
}