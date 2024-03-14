using Microsoft.AspNetCore.Mvc;

namespace booking.Controllers
{
    public class PackagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
