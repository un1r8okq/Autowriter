using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Autowriter.Features.Upload
{
    public class UploadController : Controller
    {
        private readonly IMediator _mediator;

        public UploadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index() =>
            View(await _mediator.Send(new Index.Query()));
    }
}
