using Autowriter.Data;
using MediatR;

namespace Autowriter.Pages.Upload
{
    public class DeleteHandler
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Handler : RequestHandler<Command>
        {
            private readonly ISourceMaterialRepository _repository;

            public Handler(ISourceMaterialRepository repository)
            {
                _repository = repository;
            }

            protected override void Handle(Command command) =>
                _repository.DeleteSource(command.Id);
        }
    }
}
