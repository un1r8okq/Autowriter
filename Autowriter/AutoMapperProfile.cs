using AutoMapper;
using Autowriter.Data;

namespace Autowriter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SourceMaterialRepository.Source, Pages.Upload.IndexHandler.Response>();
            CreateMap<SourceMaterialRepository.Source, Pages.Upload.DetailsHandler.Response>();
            CreateMap<SourceMaterialRepository.Source, Pages.Upload.CreateHandler.Response.Source>();
        }
    }
}
