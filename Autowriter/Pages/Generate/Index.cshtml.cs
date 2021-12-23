using AutoMapper;
using Autowriter.Features.SourceMaterial;
using Autowriter.Features.WritingGeneration.Generate;
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

        public async Task OnGetAsync()
        {
            var sourceCount = await _mediator.Send(new Count.Query());

            Data = new ViewModel
            {
                NumberOfSources = sourceCount,
                RequestedNumberOfWords = 100,
            };
        }

        public async Task OnPostAsync(GenerateHandler.Command command)
        {
            var sourceCount = await _mediator.Send(new Count.Query());
            var generateWritingResponse = await _mediator.Send(command);

            Data = new ViewModel
            {
                NumberOfSources = sourceCount,
                RequestedNumberOfWords = command.WordCount,
                WordCountOutOfRange = generateWritingResponse.WordCountOutOfRange,
                Writing = _mapper.Map<ViewModel.GeneratedWriting>(generateWritingResponse.Writing),
            };
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

                public string Content { get; set; } = string.Empty;
            }
        }
    }
}
