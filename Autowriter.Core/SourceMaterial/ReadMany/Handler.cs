using AutoMapper;
using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadMany
    {
        public class Handler : RequestHandler<Query, IEnumerable<Response>>
        {
            private readonly IReadSourceMaterials _sourceReader;
            private readonly IMapper _mapper;

            public Handler(IReadSourceMaterials sourceReader, IMapper mapper)
            {
                _mapper = mapper;
                _sourceReader = sourceReader;
            }

            protected override IEnumerable<Response> Handle(Query request) =>
                _mapper.Map<IEnumerable<Response>>(_sourceReader.GetSources());
        }
    }
}
