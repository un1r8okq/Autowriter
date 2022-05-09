using AutoMapper;
using Autowriter.Data;
using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Create
    {
        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly ICreateSourceMaterials _sourceCreator;
            private readonly IMapper _mapper;

            public Handler(ICreateSourceMaterials sourceCreator, IMapper mapper)
            {
                _sourceCreator = sourceCreator;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
            {
                if (command.Content is null || command.Content == string.Empty)
                {
                    return new Response { TextWasEmpty = true };
                }

                var createdSource = await _sourceCreator.CreateSourceAsync(DateTime.UtcNow, command.Content, cancellationToken);

                return new Response
                {
                    TextWasEmpty = false,
                    CreatedSource = _mapper.Map<Response.SourceMaterial>(createdSource),
                };
            }
        }
    }
}
