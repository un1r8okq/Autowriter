namespace Autowriter.Features.SourceMaterial
{
    public class ConfigureServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<Create.ICreateSourceMaterial, Create.Repository>();

            services.AddSingleton<SourceMaterialRepository>();
            services.AddSingleton<IReadSourceMaterials>(x => x.GetRequiredService<SourceMaterialRepository>());
            services.AddSingleton<IDeleteSourceMaterial>(x => x.GetRequiredService<SourceMaterialRepository>());
        }
    }
}
