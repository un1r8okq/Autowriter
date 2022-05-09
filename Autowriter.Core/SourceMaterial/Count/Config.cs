using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Count
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Count));
        }
    }
}
