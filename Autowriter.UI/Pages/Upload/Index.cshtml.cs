using AutoMapper;
using Autowriter.Core.Features.SourceMaterial;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Autowriter.UI.Pages.Upload
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

        public async Task OnGetAsync(ReadMany.Query query)
        {
            var sources = await _mediator.Send(query);

            Data = new ViewModel
            {
                Sources = _mapper.Map<IEnumerable<ViewModel.Source>>(sources),
            };
        }

        public async Task OnPostAsync(Create.Command command)
        {
            var createSourceResponse = await _mediator.Send(command);
            var sources = await _mediator.Send(new ReadMany.Query());

            Data = new ViewModel
            {
                TextWasEmpty = createSourceResponse.TextWasEmpty,
                Sources = _mapper.Map<IEnumerable<ViewModel.Source>>(sources),
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
