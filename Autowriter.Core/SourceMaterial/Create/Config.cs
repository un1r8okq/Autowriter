using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Create
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICreateSourceMaterial, Repository>();
            services.AddMediatR(typeof(Create));
            services.AddAutoMapper(typeof(Create));
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
