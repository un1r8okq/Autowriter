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

            services.AddSingleton<Features.WritingGeneration.IReadSourceMaterial, Features.WritingGeneration.SourceMaterialReader>();

            services.AddSingleton< Features.SourceMaterial.SourceMaterialRepository >();
            services.AddSingleton<Features.SourceMaterial.ICreateSourceMaterial>(x => x.GetRequiredService<Features.SourceMaterial.SourceMaterialRepository>());
            services.AddSingleton<Features.SourceMaterial.IReadSourceMaterials>(x => x.GetRequiredService<Features.SourceMaterial.SourceMaterialRepository>());
            services.AddSingleton< Features.SourceMaterial.IDeleteSourceMaterial >(x => x.GetRequiredService<Features.SourceMaterial.SourceMaterialRepository>());

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
