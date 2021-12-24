using AutoMapper;

namespace Autowriter.Features.SourceMaterial
{
    public partial class ReadMany
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IReadSourceMaterials, Repository>();
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
