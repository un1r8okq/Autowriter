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
            private readonly IReadSourceMaterials _repository;
            private readonly IMapper _mapper;

            public Handler(IReadSourceMaterials repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            protected override IEnumerable<Response> Handle(Query request) => _repository
                .GetSources()
                .Select(source => _mapper.Map<Response>(source));
        }
    }
}
