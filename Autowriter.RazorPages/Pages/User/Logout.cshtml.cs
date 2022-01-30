using Autowriter.RazorPages.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.RazorPages.Pages.User
{
    public class Logout : PageModel
    {
        private readonly ILogger<Register> _logger;
        private readonly SignInManager<AutowriterUser> _signInManager;

        public Logout(
            ILogger<Register> logger,
            SignInManager<AutowriterUser> signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;
        }

        public Task OnGetAsync()
        {
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPost(string? returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
