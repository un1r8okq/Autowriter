namespace Autowriter.Features.SourceMaterial
{
    public partial class Count
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICountSourceMaterials, Repository>();
        }
    }
}
