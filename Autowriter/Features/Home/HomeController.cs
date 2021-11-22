using Microsoft.AspNetCore.Mvc;

namespace Autowriter.Features.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
