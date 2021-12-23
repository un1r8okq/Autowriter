using AutoMapper;

namespace Autowriter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Pages.Upload.CreateHandler.Response.Source, Pages.Upload.Index.ViewModel.Source>();
            CreateMap<Pages.Upload.IndexHandler.Response, Pages.Upload.Index.ViewModel.Source>();
            CreateMap<Pages.Upload.SourceMaterial, Pages.Upload.IndexHandler.Response>();
            CreateMap<Pages.Upload.SourceMaterial, Pages.Upload.DetailsHandler.Response>();
            CreateMap<Pages.Upload.SourceMaterial, Pages.Upload.CreateHandler.Response.Source>();

            CreateMap<Features.WritingGeneration.GenerateHandler.Response, Pages.Generate.Index.ViewModel>();
            CreateMap<Features.WritingGeneration.GenerateHandler.Response.GeneratedWriting, Pages.Generate.Index.ViewModel.GeneratedWriting>();
        }
    }
}
