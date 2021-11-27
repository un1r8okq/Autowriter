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
                SourceMaterials = writings.Select(t => new ViewModel.SourceMaterial
                {
                    Created = t.Created,
                    Content = t.Text,
                }),
            };
        }

        public async Task OnPostAsync(Create.Command command)
        {
            var writings = await _mediator.Send(command);

            Data = new ViewModel
            {
                SourceMaterials = writings.Select(t => new ViewModel.SourceMaterial
                {
                    Created = t.Created,
                    Content = t.Text,
                }),
            };
        }

        public class ViewModel
        {
            public IEnumerable<SourceMaterial> SourceMaterials { get; set; } = Array.Empty<SourceMaterial>();

            public class SourceMaterial
            {
                public DateTime Created { get; set; }

                public string Content { get; set; } = string.Empty;
            }
        }
    }
}
