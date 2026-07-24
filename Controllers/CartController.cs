using Microsoft.AspNetCore.Mvc;

namespace BookStoreMVC.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}