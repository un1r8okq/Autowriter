using AutoMapper;

namespace Autowriter.Features.SourceMaterial
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<ReadMany.Response, Pages.Upload.Index.ViewModel.Source>();
            CreateMap<SourceMaterial, ReadMany.Response>();
            CreateMap<SourceMaterial, ReadSingle.Response>();
            CreateMap<SourceMaterial, Create.Response.SourceMaterial>();
        }
    }
}
