namespace Autowriter.Features.WritingGeneration
{
    public class ConfigureServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<Generate.IReadSourceMaterial, Generate.SourceMaterialReader>();
        }
    }
}
