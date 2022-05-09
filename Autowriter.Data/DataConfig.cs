using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Data
{
    public static class DataConfig
    {
        public static void RegisterAutowriterDataServices(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlite("Data source=AutowriterDataEntityFramework.sqlite"));

            services.AddTransient<ICreateSourceMaterials, SourceMaterialRepository>();
            services.AddTransient<ICountSourceMaterials, SourceMaterialRepository>();
            services.AddTransient<IReadSourceMaterial, SourceMaterialRepository>();
            services.AddTransient<IReadSourceMaterials, SourceMaterialRepository>();
            services.AddTransient<IDeleteSourceMaterial, SourceMaterialRepository>();
        }

        public static void EnsureDbCreated(this IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<DataContext>().Database.EnsureCreated();
        }
    }
}
