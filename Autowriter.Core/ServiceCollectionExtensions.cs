using Autowriter.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAutowriterCoreServices(this IServiceCollection services)
        {
            services.RegisterAutowriterDataServices();

            Features.SourceMaterial.Count.ConfigureServices(services);
            Features.SourceMaterial.Create.ConfigureServices(services);
            Features.SourceMaterial.Delete.ConfigureServices(services);
            Features.SourceMaterial.ReadSingle.ConfigureServices(services);
            Features.SourceMaterial.ReadMany.ConfigureServices(services);
            Features.WritingGeneration.Generate.ConfigureServices(services);
        }
    }
}
