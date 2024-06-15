using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated) return View();
        else return RedirectToAction("Login", "Authorization");
    }
}
