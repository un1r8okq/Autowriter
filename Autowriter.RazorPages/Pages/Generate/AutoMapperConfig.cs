using AutoMapper;

namespace Autowriter.RazorPages.Pages.Generate
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Core.Features.WritingGeneration.Generate.Response.GeneratedWriting, Index.ViewModel.GeneratedWriting>();
        }
    }
}
