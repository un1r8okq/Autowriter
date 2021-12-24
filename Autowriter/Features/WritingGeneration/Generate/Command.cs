using MediatR;

namespace Autowriter.Features.WritingGeneration
{
    public partial class Generate
    {
        public class Command : IRequest<Response>
        {
            public int WordCount { get; set; }
        }
    }
}
