using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
        public class Handler : RequestHandler<Command, Response>
        {
            private readonly ICreateSourceMaterial _sourceCreator;

            public Handler(ICreateSourceMaterial sourceCreator)
            {
                _sourceCreator = sourceCreator;
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
                    CreatedSource = createdSource,
                };
            }
        }
    }
}
