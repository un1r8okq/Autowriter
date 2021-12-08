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
            private readonly ICreateSourceMaterial _createRepo;
            private readonly IReadSourceMaterials _readRepo;
            private readonly IMapper _mapper;

            public Handler(ICreateSourceMaterial createRepo, IReadSourceMaterials readRepo, IMapper mapper)
            {
                _mapper = mapper;
                _createRepo = createRepo;
                _readRepo = readRepo;
            }

            protected override Response Handle(Command command)
            {
                if (command.Content is null || command.Content == string.Empty)
                {
                    return new Response { TextWasEmpty = true };
                }

                _createRepo.CreateSource(DateTime.UtcNow, command.Content);

                return new Response
                {
                    TextWasEmpty = false,
                    Sources = _readRepo
                        .GetSources()
                        .Select(source => _mapper.Map<Response.Source>(source)),
                };
            }
        }
    }
}
