using AutoMapper;

namespace Autowriter.Pages.Upload
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Core.Features.SourceMaterial.ReadMany.Response, Index.ViewModel.Source>();
        }
    }
}
