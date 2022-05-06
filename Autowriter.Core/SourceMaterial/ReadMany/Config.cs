using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadMany
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IReadSourceMaterials, Repository>();
            services.AddAutoMapper(typeof(ReadMany));
        }

        public class AutoMapper : Profile
        {
            public AutoMapper()
            {
                CreateMap<Repository.SourceMaterial, Response>();
            }
        }
    }
}
