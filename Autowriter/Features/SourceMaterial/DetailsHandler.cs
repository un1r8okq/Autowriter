using AutoMapper;
using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public class DetailsHandler
    {
        public class Query : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; } = string.Empty;
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IReadSourceMaterials _sourceReader;
            private readonly IMapper _mapper;

            public Handler(IReadSourceMaterials sourceReader, IMapper mapper)
            {
                _mapper = mapper;
                _sourceReader = sourceReader;
            }

            protected override Response Handle(Query request) =>
                _mapper.Map<Response>(_sourceReader.GetSource(request.Id));
        }
    }
}
