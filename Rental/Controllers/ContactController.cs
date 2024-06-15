using Microsoft.AspNetCore.Mvc;

namespace Rental.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
