using Autowriter.Data;
using MediatR;

namespace Autowriter.Pages.Generate
{
    public class IndexHandler
    {
        public class Query : IRequest<Response> { }

        public class Response
        {
            public int SourceCount { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IReadSourceMaterials _sourceReader;

            public Handler(IReadSourceMaterials sourceReader)
            {
                _sourceReader = sourceReader;
            }

            protected override Response Handle(Query request) =>
                new()
                {
                    SourceCount = _sourceReader.GetSources().Count(),
                };
        }
    }
}
