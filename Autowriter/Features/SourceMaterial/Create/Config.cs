using AutoMapper;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICreateSourceMaterial, Repository>();
        }

        public class AutoMapper : Profile
        {
            public AutoMapper()
            {
                CreateMap<Repository.SourceMaterial, Response.SourceMaterial>();
            }
        }
    }
}
