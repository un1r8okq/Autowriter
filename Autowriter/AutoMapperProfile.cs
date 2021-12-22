using AutoMapper;
using Autowriter.Data;

namespace Autowriter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Pages.Upload.IndexHandler.Response, Pages.Upload.Index.ViewModel.Source>();
            CreateMap<Pages.Upload.SourceMaterial, Pages.Upload.IndexHandler.Response>();
            CreateMap<Pages.Upload.SourceMaterial, Pages.Upload.DetailsHandler.Response>();
            CreateMap<Pages.Upload.SourceMaterial, Pages.Upload.CreateHandler.Response.Source>();

            CreateMap<Pages.Generate.GenerateHandler.Response, Pages.Generate.Index.ViewModel>();
            CreateMap<Pages.Generate.GenerateHandler.Response.GeneratedWriting, Pages.Generate.Index.ViewModel.GeneratedWriting>();
        }
    }
}
