using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

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
                    Content = t.Content,
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
