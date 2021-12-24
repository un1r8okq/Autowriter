namespace Autowriter.Features.SourceMaterial
{
    public class ConfigureServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<Create.ICreateSourceMaterial, Create.Repository>();
            services.AddSingleton<Delete.IDeleteSourceMaterial, Delete.Repository>();
            services.AddSingleton<IReadSourceMaterials, SourceMaterialRepository>();
        }
    }
}
