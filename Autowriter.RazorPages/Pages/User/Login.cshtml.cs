using Autowriter.RazorPages.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.RazorPages.Pages.User
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
        }

        public IList<string> RegisteredUserNames => _userManager.Users.Select(u => u.UserName).ToList();

        public async Task OnPostAsync(string email, string password)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user != null)
            {
                var passwordCorrect = await _userManager.CheckPasswordAsync(user, password);

                if (passwordCorrect)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                }
            }
        }
    }
}
