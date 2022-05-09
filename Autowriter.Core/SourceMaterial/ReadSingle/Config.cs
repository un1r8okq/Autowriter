using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadSingle
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ReadSingle));
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
