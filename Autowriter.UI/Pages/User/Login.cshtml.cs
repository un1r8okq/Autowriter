using Autowriter.UI.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            Data = new ViewModel();
        }

        public ViewModel Data { get; set; }

        public void OnGet()
        {
            Data = new ViewModel
            {
                LoginFailed = false,
            };
        }

        public async Task OnPostAsync(string email, string password)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user is null)
            {
                Data = new ViewModel
                {
                    LoginFailed = true,
                };
                return;
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (passwordCorrect)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                Data = new ViewModel
                {
                    LoginFailed = false,
                };
                Redirect("/");
                return;
            }

            Data = new ViewModel
            {
                LoginFailed = true,
            };
        }

        public class ViewModel
        {
            public bool LoginFailed { get; set; }
        }
    }
}
