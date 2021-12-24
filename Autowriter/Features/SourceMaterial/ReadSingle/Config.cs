using AutoMapper;

namespace Autowriter.Features.SourceMaterial
{
    public partial class ReadSingle
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IReadSourceMaterial, Repository>();
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
