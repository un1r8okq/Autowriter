using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Autowriter.UI.Identity
{
    public class AutowriterIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public AutowriterIdentityDbContext(DbContextOptions<AutowriterIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUser>()
                   .ToTable("AutowriterUsers");
            base.OnModelCreating(builder);
        }
    }
}
