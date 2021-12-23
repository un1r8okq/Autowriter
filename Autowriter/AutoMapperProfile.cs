using AutoMapper;

namespace Autowriter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Features.SourceMaterial.Create.Response.Source, Pages.Upload.Index.ViewModel.Source>();
            CreateMap<Features.SourceMaterial.ReadMany.Response, Pages.Upload.Index.ViewModel.Source>();
            CreateMap<Features.SourceMaterial.SourceMaterial, Features.SourceMaterial.ReadMany.Response>();
            CreateMap<Features.SourceMaterial.SourceMaterial, Features.SourceMaterial.ReadSingle.Response>();
            CreateMap<Features.SourceMaterial.SourceMaterial, Features.SourceMaterial.Create.Response.Source>();

            CreateMap<Features.WritingGeneration.GenerateHandler.Response, Pages.Generate.Index.ViewModel>();
            CreateMap<Features.WritingGeneration.GenerateHandler.Response.GeneratedWriting, Pages.Generate.Index.ViewModel.GeneratedWriting>();
        }
    }
}
