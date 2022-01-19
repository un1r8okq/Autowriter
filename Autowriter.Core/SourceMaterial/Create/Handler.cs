using AutoMapper;
using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Create
    {
        public class Handler : RequestHandler<Command, Response>
        {
            private readonly ICreateSourceMaterial _sourceCreator;
            private readonly IMapper _mapper;

            public Handler(ICreateSourceMaterial sourceCreator, IMapper mapper)
            {
                _sourceCreator = sourceCreator;
                _mapper = mapper;
            }

            protected override Response Handle(Command command)
            {
                if (command.Content is null || command.Content == string.Empty)
                {
                    return new Response { TextWasEmpty = true };
                }

                var createdSource = _sourceCreator.CreateSource(DateTime.UtcNow, command.Content);

                return new Response
                {
                    TextWasEmpty = false,
                    CreatedSource = _mapper.Map<Response.SourceMaterial>(createdSource),
                };
            }
        }
    }
}
