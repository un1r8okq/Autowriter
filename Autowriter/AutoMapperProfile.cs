using AutoMapper;
using Autowriter.Data;

namespace Autowriter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SourceMaterial, Pages.Upload.IndexHandler.Response>();
            CreateMap<SourceMaterial, Pages.Upload.DetailsHandler.Response>();
            CreateMap<SourceMaterial, Pages.Upload.CreateHandler.Response.Source>();

            CreateMap<Pages.Generate.GenerateHandler.Response, Pages.Generate.Index.ViewModel>();
            CreateMap<Pages.Generate.GenerateHandler.Response.GeneratedWriting, Pages.Generate.Index.ViewModel.GeneratedWriting>();
        }
    }
}
