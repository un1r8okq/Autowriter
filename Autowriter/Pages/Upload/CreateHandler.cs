using AutoMapper;
using MediatR;

namespace Autowriter.Pages.Upload
{
    public class CreateHandler
    {
        public class Command : IRequest<Response>
        {
            public string Content { get; set; } = string.Empty;
        }

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

        public class Handler : RequestHandler<Command, Response>
        {
            private readonly ICreateSourceMaterial _sourceCreator;
            private readonly IReadSourceMaterials _sourceReader;
            private readonly IMapper _mapper;

            public Handler(ICreateSourceMaterial sourceCreator, IReadSourceMaterials sourceReader, IMapper mapper)
            {
                _sourceCreator = sourceCreator;
                _sourceReader = sourceReader;
                _mapper = mapper;
            }

            protected override Response Handle(Command command)
            {
                if (command.Content is null || command.Content == string.Empty)
                {
                    return new Response { TextWasEmpty = true };
                }

                _sourceCreator.CreateSource(DateTime.UtcNow, command.Content);

                return new Response
                {
                    TextWasEmpty = false,
                    Sources = _mapper.Map<IEnumerable<Response.Source>>(_sourceReader.GetSources()),
                };
            }
        }
    }
}
