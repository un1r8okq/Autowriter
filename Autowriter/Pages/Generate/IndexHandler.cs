using Autowriter.Data;
using MediatR;

namespace Autowriter.Pages.Generate
{
    public class IndexHandler
    {
        public class Model
        {
            public int SourceCount { get; set; }
        }

        public class Query : IRequest<Model> { }

        public class Handler : RequestHandler<Query, Model>
        {
            private readonly ISourceMaterialRepository _repository;

            public Handler(ISourceMaterialRepository repository)
            {
                _repository = repository;
            }

            protected override Model Handle(Query request) =>
                new()
                {
                    SourceCount = _repository.GetSources().Count(),
                };
        }
    }
}
