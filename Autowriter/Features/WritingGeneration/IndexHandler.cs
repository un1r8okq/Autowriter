using MediatR;

namespace Autowriter.Features.WritingGeneration
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
            private readonly IReadSourceMaterial _sourceReader;

            public Handler(IReadSourceMaterial sourceReader)
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
