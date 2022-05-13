using Autowriter.Core;
using Autowriter.Data;
using Autowriter.UI.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Autowriter.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterAutowriterCoreServices();

            services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlite(
                        Configuration.GetConnectionString("IdentityContextConnection")));

            services.AddDefaultIdentity<AutowriterUIUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdentityContext>();

            services.AddAutoMapper(typeof(Program));

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            using var scope = app.ApplicationServices.CreateScope();
            scope.ServiceProvider.EnsureDbCreated();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapRazorPages());
            app.UseStatusCodePages();
        }
    }
}
