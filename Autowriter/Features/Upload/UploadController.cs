using Microsoft.AspNetCore.Mvc;

namespace Autowriter.Features.Upload
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
