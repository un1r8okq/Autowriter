using MediatR;

namespace Autowriter.Features.WritingGeneration.Generate
{
    public partial class GenerateHandler
    {
        public class Command : IRequest<Response>
        {
            public int WordCount { get; set; }
        }
    }
}
