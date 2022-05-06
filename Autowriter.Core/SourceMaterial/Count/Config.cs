using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Count
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICountSourceMaterials, Repository>();
            services.AddMediatR(typeof(Count));
        }
    }
}
