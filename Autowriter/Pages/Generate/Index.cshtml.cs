using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.Pages.Generate
{
    public class Index : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public Index(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
            Data = new ViewModel();
        }

        public ViewModel Data { get; private set; }

        public async Task OnGetAsync(IndexHandler.Query query)
        {
            var sources = await _mediator.Send(query);

            Data = new ViewModel
            {
                NumberOfSources = sources.SourceCount,
                RequestedNumberOfWords = 100,
            };
        }

        public async Task OnPostAsync(GenerateHandler.Command command)
        {
            var response = await _mediator.Send(command);
            Data = _mapper.Map<ViewModel>(response);
        }

        public class ViewModel
        {
            public int NumberOfSources { get; set; }

            public int RequestedNumberOfWords { get; set; }

            public bool WordCountOutOfRange { get; set; }

            public GeneratedWriting? Writing { get; set; }

            public class GeneratedWriting
            {
                public int Id { get; set; }

                public DateTime Created { get; set; }

                public string Content { get; set; } = String.Empty;
            }
        }
    }
}
