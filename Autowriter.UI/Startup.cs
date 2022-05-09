using Autowriter.Core;
using Autowriter.Data;
using Autowriter.UI.Identity;
using Microsoft.AspNetCore.Identity;
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

            services.AddAutoMapper(typeof(Program));

            ConfigureIdentityServices(services);

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
            scope.ServiceProvider.GetRequiredService<AutowriterIdentityDbContext>().Database.EnsureCreated();

            scope.ServiceProvider.EnsureDbCreated();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapRazorPages());
            app.UseStatusCodePages();
        }

        private void ConfigureIdentityServices(IServiceCollection services)
        {
            var dbConnectionString = Configuration.GetSection("IdentityDatabaseName").Value;
            services.AddDbContext<AutowriterIdentityDbContext>(options => options.UseSqlite(dbConnectionString));

            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<AutowriterIdentityDbContext>();
        }
    }
}
