using AutoMapper;
using Autowriter.Data;
using MediatR;

namespace Autowriter.Pages.Upload
{
    public class CreateHandler
    {
        public class Response
        {
            public bool TextWasEmpty { get; set; }

            public IEnumerable<Source> Sources { get; set; } = Array.Empty<Source>();

            public class Source
            {
                public int Id { get; set; }

                public DateTime Created { get; set; }

                public string Content { get; set; } = string.Empty;
            }
        }

        public class Command : IRequest<Response>
        {
            public string Content { get; set; } = string.Empty;
        }

        public class Handler : RequestHandler<Command, Response>
        {
            private readonly ISourceMaterialRepository _repository;
            private readonly IMapper _mapper;

            public Handler(ISourceMaterialRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            protected override Response Handle(Command command)
            {
                if (command.Content is null || command.Content == string.Empty)
                {
                    return new Response { TextWasEmpty = true };
                }

                _repository.CreateSource(DateTime.UtcNow, command.Content);

                return new Response
                {
                    TextWasEmpty = false,
                    Sources = _repository
                        .GetSources()
                        .Select(source => _mapper.Map<Response.Source>(source)),
                };
            }
        }
    }
}
