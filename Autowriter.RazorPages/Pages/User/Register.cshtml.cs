using System.ComponentModel.DataAnnotations;
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
            Email = string.Empty;
            Password = string.Empty;
            Errors = new List<string>();
        }

        [Required]
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }

        [Required]
        [BindProperty]
        public string Password { get; set; }

        public IList<string> Errors { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var errors = new List<string>();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await CreateUser(Email, Password);
                errors.AddRange(result.Errors.Select(error => error.Description));
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            if (errors.Count > 0)
            {
                _logger.LogError("Failed to create user. Error(s): {errors}", string.Join(',', errors));
                Errors = errors;
                return Page();
            }
            else
            {
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
    }
}
