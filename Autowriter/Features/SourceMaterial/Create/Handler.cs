using AutoMapper;
using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
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
