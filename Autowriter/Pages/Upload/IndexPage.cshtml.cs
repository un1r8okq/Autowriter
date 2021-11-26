using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Autowriter.Pages.Upload
{
    public class IndexPage : PageModel
    {
        private readonly IMediator _mediator;

        public IndexPage(IMediator mediator)
        {
            _mediator = mediator;
            Data = new ViewModel();
        }

        public ViewModel Data { get; private set; }

        public async Task OnGetAsync(Index.Query query)
        {
            var writings = await _mediator.Send(query);

            Data = new ViewModel
            {
                Writings = writings
                    .Select(t => new ViewModel.Writing
                    {
                        Created = t.Created.ToLocalTime(),
                        Content = t.Text,
                    })
                    .OrderByDescending(t => t.Created),
            };
        }

        public async Task OnPostAsync(Create.Command command)
        {
            var writings = await _mediator.Send(command);

            Data = new ViewModel
            {
                Writings = writings
                    .Select(t => new ViewModel.Writing
                    {
                        Created = t.Created.ToLocalTime(),
                        Content = t.Text,
                    })
                    .OrderByDescending(t => t.Created),
            };
        }

        public class ViewModel
        {
            public IEnumerable<Writing> Writings { get; set; } = Array.Empty<Writing>();

            public class Writing
            {
                public DateTime Created { get; set; }

                public string Content { get; set; } = string.Empty;
            }
        }
    }
}
