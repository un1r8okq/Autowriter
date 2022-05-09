using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadMany
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ReadMany));
        }

        public class AutoMapper : Profile
        {
            public AutoMapper()
            {
                CreateMap<Autowriter.Data.Models.SourceMaterial, Response>();
            }
        }
    }
}
