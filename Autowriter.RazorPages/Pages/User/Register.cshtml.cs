using Autowriter.RazorPages.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.RazorPages.Pages.User
{
    [AllowAnonymous]
    public class Register : PageModel
    {
        private readonly ILogger<Register> _logger;
        private readonly UserManager<AutowriterUser> _userManager;

        public Register(
            ILogger<Register> logger,
            UserManager<AutowriterUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            Data = new ViewModel();
        }

        public ViewModel Data { get; set; }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            var errors = new List<string>();
            try
            {
                var result = await CreateUser(email, password);
                errors.AddRange(result.Errors.Select(error => error.Description));
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            if (errors.Count > 0)
            {
                _logger.LogError("Failed to create user. Error(s): {errors}", string.Join(',', errors));
                Data = new ViewModel { Errors = errors };
                return Page();
            }
            else
            {
                Data = new ViewModel();
                return Redirect("/user/login");
            }
        }

        private async Task<IdentityResult> CreateUser(string email, string password)
        {
            var userToCreate = new AutowriterUser
            {
                UserName = email,
            };
            var result = await _userManager.CreateAsync(userToCreate);

            if (!result.Succeeded)
            {
                return result;
            }

            var createdUser = await _userManager.FindByNameAsync(email);
            if (createdUser == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Failed to get {typeof(AutowriterUser)} by userName after creation",
                });
            }

            var addPassResult = await _userManager.AddPasswordAsync(createdUser, password);

            if (!addPassResult.Succeeded)
            {
                return addPassResult;
            }

            return IdentityResult.Success;
        }

        public class ViewModel
        {
            public ViewModel()
            {
                Errors = Array.Empty<string>();
            }

            public IList<string> Errors { get; set; }
        }
    }
}
