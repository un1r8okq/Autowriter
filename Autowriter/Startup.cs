using System.Data;
using MediatR;
using Microsoft.Data.Sqlite;

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

            Features.SourceMaterial.Count.ConfigureServices(services);
            Features.SourceMaterial.Create.ConfigureServices(services);
            Features.SourceMaterial.Delete.ConfigureServices(services);
            Features.SourceMaterial.ReadMany.ConfigureServices(services);
            Features.SourceMaterial.ReadSingle.ConfigureServices(services);
            Features.WritingGeneration.Generate.ConfigureServices(services);

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
