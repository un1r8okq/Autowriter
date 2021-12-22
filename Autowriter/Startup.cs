using Autowriter.Data;
using Autowriter.Pages;
using MediatR;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Autowriter
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
            services.AddSingleton<IDbConnection>((serviceProvider) =>
                new SqliteConnection(Configuration.GetSection("DatabaseName").Value));

            services.AddSingleton<Pages.Generate.IReadSourceMaterial, Pages.Generate.SourceMaterialReader>();

            services.AddSingleton<Pages.Upload.SourceMaterialRepository>();
            services.AddSingleton<Pages.Upload.ICreateSourceMaterial>(x => x.GetRequiredService<Pages.Upload.SourceMaterialRepository>());
            services.AddSingleton<Pages.Upload.IReadSourceMaterials>(x => x.GetRequiredService<Pages.Upload.SourceMaterialRepository>());
            services.AddSingleton<Pages.Upload.IDeleteSourceMaterial>(x => x.GetRequiredService<Pages.Upload.SourceMaterialRepository>());

            services.AddMediatR(typeof(Program));
            services.AddAutoMapper(typeof(Program));
            services.AddRazorPages();
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
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapRazorPages());
            app.UseStatusCodePages();
        }
    }
}
