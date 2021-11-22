using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Autowriter.Features.Home
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index() =>
            View(await _mediator.Send(new Index.Query()));
    }
}
