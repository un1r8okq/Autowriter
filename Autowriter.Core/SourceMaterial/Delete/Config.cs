using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Delete
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDeleteSourceMaterial, Repository>();
            services.AddAutoMapper(typeof(Delete));
        }
    }
}