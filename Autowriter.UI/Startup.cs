using Autowriter.Core;
using Autowriter.UI.Identity;
using Microsoft.AspNetCore.Identity;

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
            var userDbConnection = new UserDbConnection(dbConnectionString);
            services.AddSingleton(userDbConnection);

            services
                .AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme, options =>
                {
                    options.LoginPath = "/User/Login";
                })
                .AddCookie(IdentityConstants.ExternalScheme)
                .AddCookie(IdentityConstants.TwoFactorUserIdScheme);

            services.AddTransient<IUserStore<AutowriterUser>, UserStore>();
            services.AddTransient<IUserPasswordStore<AutowriterUser>, UserStore>();
            services
                .AddIdentityCore<AutowriterUser>(options =>
                {
                    options.Password.RequiredLength = 1;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                })
                .AddSignInManager();
        }
    }
}
