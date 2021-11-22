using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.Features.Home
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ViewModel Data { get; private set; }

        public async Task GetTaskAsync(Query query)
        {
            Data = await _mediator.Send(query);
        }

        public class ViewModel { }

        public class Query : IRequest<ViewModel> {}

        public class Handler : IRequestHandler<Query, ViewModel>
        {
            public Task<ViewModel> Handle(Query request, CancellationToken cancellationToken)
                => Task.FromResult(new ViewModel());
        }
    }
}
