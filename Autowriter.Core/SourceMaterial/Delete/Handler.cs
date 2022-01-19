using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Delete
    {
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
