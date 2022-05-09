using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Create
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(Create));
            services.AddAutoMapper(typeof(Create));
        }

        public class AutoMapper : Profile
        {
            public AutoMapper()
            {
                CreateMap<Autowriter.Data.Models.SourceMaterial, Response.SourceMaterial>();
            }
        }
    }
}
