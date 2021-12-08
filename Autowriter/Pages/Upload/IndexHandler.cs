using MediatR;
using Autowriter.Data;
using AutoMapper;

namespace Autowriter.Pages.Upload
{
    public class IndexHandler
    {
        public class Query : IRequest<IEnumerable<Response>> { }

        public class Response
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; } = string.Empty;
        }

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
