namespace Autowriter.Features.SourceMaterial
{
    public partial class Delete
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDeleteSourceMaterial, Repository>();
        }
    }
}
