using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.RazorPages.Pages
{
    [AllowAnonymous]
    public class Index : PageModel
    {
        public void OnGet()
        {
        }
    }
}
