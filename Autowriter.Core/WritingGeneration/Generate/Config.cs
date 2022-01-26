using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.WritingGeneration
{
    public partial class Generate
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IReadSourceMaterial, Repository>();
            services.AddMediatR(typeof(Generate));
        }
    }
}
