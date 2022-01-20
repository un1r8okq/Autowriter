using AutoMapper;
using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadSingle
    {
        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IReadSourceMaterial _sourceReader;
            private readonly IMapper _mapper;

            public Handler(IReadSourceMaterial sourceReader, IMapper mapper)
            {
                _mapper = mapper;
                _sourceReader = sourceReader;
            }

            protected override Response Handle(Query request) =>
                _mapper.Map<Response>(_sourceReader.GetSource(request.Id));
        }
    }
}
