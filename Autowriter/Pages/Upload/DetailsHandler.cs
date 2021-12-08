using AutoMapper;
using Autowriter.Data;
using MediatR;

namespace Autowriter.Pages.Upload
{
    public class DetailsHandler
    {
        public class Response
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; } = string.Empty;
        }

        public class Query : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IReadSourceMaterials _repository;
            private readonly IMapper _mapper;

            public Handler(IReadSourceMaterials repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            protected override Response Handle(Query request) =>
                _mapper.Map<Response>(_repository.GetSource(request.Id));
        }
    }
}
