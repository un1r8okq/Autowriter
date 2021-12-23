using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Autowriter.Features.SourceMaterial;

namespace Autowriter.Pages.Upload
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
                Sources = _mapper.Map<IEnumerable<ViewModel.Source>>(sources),
            };
        }

        public async Task OnPostAsync(Create.Command command)
        {
            var result = await _mediator.Send(command);

            Data = new ViewModel
            {
                TextWasEmpty = result.TextWasEmpty,
                Sources = _mapper.Map<IEnumerable<ViewModel.Source>>(result.Sources),
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
