using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public class DeleteHandler
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : RequestHandler<Command>
        {
            private readonly IDeleteSourceMaterial _sourceDeletor;

            public Handler(IDeleteSourceMaterial sourceDeletor)
            {
                _sourceDeletor = sourceDeletor;
            }

            protected override void Handle(Command command) =>
                _sourceDeletor.DeleteSource(command.Id);
        }
    }
}
