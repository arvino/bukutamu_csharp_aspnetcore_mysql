using Microsoft.AspNetCore.Mvc;

namespace BukuTamuApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "BukuTamu");
        }

        [Route("/Home/Error")]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
} 