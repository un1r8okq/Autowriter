using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadSingle
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IReadSourceMaterial, Repository>();
            services.AddAutoMapper(typeof(ReadSingle));
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
