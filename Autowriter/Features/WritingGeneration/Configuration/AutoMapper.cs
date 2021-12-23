using AutoMapper;

namespace Autowriter.Features.WritingGeneration
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Generate.GenerateHandler.Response.GeneratedWriting, Pages.Generate.Index.ViewModel.GeneratedWriting>();
        }
    }
}
