namespace Autowriter.Features.WritingGeneration
{
    public partial class Generate
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IReadSourceMaterial, Repository>();
        }
    }
}
