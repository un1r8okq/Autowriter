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

        public async Task OnGetAsync(IndexHandler.Query query)
        {
            var sources = await _mediator.Send(query);

            Data = new ViewModel
            {
                Sources = sources.Select(t => new ViewModel.Source
                {
                    Id = t.Id,
                    Created = t.Created,
                    Content = t.Text,
                }),
            };
        }

        public async Task OnPostAsync(CreateHandler.Command command)
        {
            var result = await _mediator.Send(command);

            Data = new ViewModel
            {
                TextWasEmpty = result.TextWasEmpty,
                Sources = result.Sources.Select(t => new ViewModel.Source
                {
                    Id = t.Id,
                    Created = t.Created,
                    Content = t.Text,
                }),
            };
        }

        public class ViewModel
        {
            public bool TextWasEmpty { get; set; }

            public IEnumerable<Source> Sources { get; set; } = Array.Empty<Source>();

            public class Source
            {
                public int Id { get; set; }

                public DateTime Created { get; set; }

                public string Content { get; set; } = string.Empty;
            }
        }
    }
}
