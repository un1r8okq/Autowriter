using Microsoft.AspNetCore.Identity;

namespace Autowriter.RazorPages.Identity
{
    public class AutowriterUser : IdentityUser<int>
    {
        public AutowriterUser()
        {
            ConcurrencyStamp = string.Empty;
            Id = -1;
            NormalizedUserName = string.Empty;
            PasswordHash = null;
            SecurityStamp = string.Empty;
            UserName = string.Empty;
        }

        public override string ConcurrencyStamp { get; set; }

        public override string Email => UserName;

        public override int Id { get; set; }

        public override string NormalizedUserName { get; set; }

        public override string? PasswordHash { get; set; }

        public override string SecurityStamp { get; set; }

        public override string UserName { get; set; }
    }
}
