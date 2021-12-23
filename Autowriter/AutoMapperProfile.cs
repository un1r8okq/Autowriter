using AutoMapper;

namespace Autowriter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Features.SourceMaterial.CreateHandler.Response.Source, Pages.Upload.Index.ViewModel.Source>();
            CreateMap<Features.SourceMaterial.IndexHandler.Response, Pages.Upload.Index.ViewModel.Source>();
            CreateMap<Features.SourceMaterial.SourceMaterial, Features.SourceMaterial.IndexHandler.Response>();
            CreateMap<Features.SourceMaterial.SourceMaterial, Features.SourceMaterial.DetailsHandler.Response>();
            CreateMap<Features.SourceMaterial.SourceMaterial, Features.SourceMaterial.CreateHandler.Response.Source>();

            CreateMap<Features.WritingGeneration.GenerateHandler.Response, Pages.Generate.Index.ViewModel>();
            CreateMap<Features.WritingGeneration.GenerateHandler.Response.GeneratedWriting, Pages.Generate.Index.ViewModel.GeneratedWriting>();
        }
    }
}
