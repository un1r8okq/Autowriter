using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.Pages.Upload
{
    public class Details : PageModel
    {
        private readonly IMediator _mediator;

        public Details(IMediator mediator)
        {
            _mediator = mediator;
            Data = new ViewModel();
        }

        public ViewModel Data { get; private set; }

        public async Task<IActionResult> OnGetAsync(DetailsHandler.Query query)
        {
            var source = await _mediator.Send(query);

            if (source == null)
            {
                return NotFound();
            }

            Data = new ViewModel
            {
                Id = source.Id,
                Created = source.Created,
                Content = source.Text,
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(DeleteHandler.Command command)
        {
            var result = await _mediator.Send(command);

            if (result.Error != string.Empty)
            {
                Data = new ViewModel
                {
                    Id = result.Id,
                    Created = result.Created,
                    Content = result.Text,
                    ErrorMessage = result.Error,
                };
                return Page();
            }
            return Redirect("/upload");
        }

        public class ViewModel
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; } = string.Empty;

            public string ErrorMessage { get; set; } = string.Empty;
        }
    }
}
