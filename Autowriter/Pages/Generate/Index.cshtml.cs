using AutoMapper;
using Autowriter.Core.Features.SourceMaterial;
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

        public async Task OnPostAsync(int wordCount)
        {
            var sourceCount = await _mediator.Send(new Count.Query());
            var generateWritingCommand = new Core.Features.WritingGeneration.Generate.Command { WordCount = wordCount };
            var generateWritingResponse = await _mediator.Send(generateWritingCommand);

            Data = new ViewModel
            {
                NumberOfSources = sourceCount,
                RequestedNumberOfWords = wordCount,
                WordCountOutOfRange = generateWritingResponse.WordCountOutOfRange,
                Writing = _mapper.Map<ViewModel.GeneratedWriting>(generateWritingResponse.Writing),
            };
        }

        public class ViewModel
        {
            public int NumberOfSources { get; set; }

            public int RequestedNumberOfWords { get; set; }

            public int MinWordCount => Core.Features.WritingGeneration.Generate.Command.MinWordCount;

            public int MaxWordCount => Core.Features.WritingGeneration.Generate.Command.MaxWordCount;

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
