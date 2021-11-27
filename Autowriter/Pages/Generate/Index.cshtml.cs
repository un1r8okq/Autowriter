using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.Pages.Generate
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator)
        {
            _mediator = mediator;
            Data = new ViewModel();
        }

        public ViewModel Data { get; private set; }

        public async Task OnGetAsync(IndexHandler.Query query)
        {
            var sources = await _mediator.Send(query);

            Data = new ViewModel
            {
                SourceCount = sources.SourceCount,
            };
        }

        public class ViewModel
        {
            public int SourceCount { get; set; }
        }
    }
}
