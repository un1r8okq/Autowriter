using AutoMapper;
using Autowriter.Data;
using Autowriter.Pages.Generate;

namespace Autowriter
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SourceMaterialRepository.Source, Pages.Upload.IndexHandler.Response>();
            CreateMap<SourceMaterialRepository.Source, Pages.Upload.DetailsHandler.Response>();
            CreateMap<SourceMaterialRepository.Source, Pages.Upload.CreateHandler.Response.Source>();

            CreateMap<GenerateHandler.Model, Pages.Generate.Index.ViewModel>();
            CreateMap<GenerateHandler.Model.GeneratedWriting, Pages.Generate.Index.ViewModel.GeneratedWriting>();
        }
    }
}
