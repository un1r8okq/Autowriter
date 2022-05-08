using System.ComponentModel.DataAnnotations;
using Autowriter.UI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.UI.Pages.User
{
    [AllowAnonymous]
    public class Login : PageModel
    {
        private readonly SignInManager<AutowriterUser> _signInManager;
        private readonly UserManager<AutowriterUser> _userManager;

        public Login(
            SignInManager<AutowriterUser> signInManager,
            UserManager<AutowriterUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;

            LoginFailed = false;
        }

        public bool LoginFailed { get; set; }

        [Required]
        [EmailAddress]
        [BindProperty]
        public string? Email { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public void OnGet(string? email)
        {
            Email = email;
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user is null)
            {
                LoginFailed = true;
                return Page();
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (passwordCorrect)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return Redirect("/");
            }

            LoginFailed = true;
            return Page();
        }
    }
}
