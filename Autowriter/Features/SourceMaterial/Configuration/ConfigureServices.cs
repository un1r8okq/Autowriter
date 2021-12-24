namespace Autowriter.Features.SourceMaterial
{
    public class ConfigureServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<Create.ICreateSourceMaterial, Create.Repository>();
            services.AddSingleton<Delete.IDeleteSourceMaterial, Delete.Repository>();
            services.AddSingleton<Count.ICountSourceMaterials, Count.Repository>();
            services.AddSingleton<ReadMany.IReadSourceMaterials, ReadMany.Repository>();
            services.AddSingleton<IReadSourceMaterials, SourceMaterialRepository>();
        }
    }
}
