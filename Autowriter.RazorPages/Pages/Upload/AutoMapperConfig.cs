using AutoMapper;

namespace Autowriter.RazorPages.Pages.Upload
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Core.Features.SourceMaterial.ReadMany.Response, Index.ViewModel.Source>();
        }
    }
}
