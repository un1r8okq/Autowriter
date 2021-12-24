using AutoMapper;

namespace Autowriter.Features.SourceMaterial
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Create.Repository.SourceMaterial, Create.Response.SourceMaterial>();
            CreateMap<ReadMany.Repository.SourceMaterial, ReadMany.Response>();
            CreateMap<ReadSingle.Repository.SourceMaterial, ReadSingle.Response>();
            CreateMap<ReadMany.Response, Pages.Upload.Index.ViewModel.Source>();
        }
    }
}
