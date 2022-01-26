using MediatR;

namespace Autowriter.Core.Features.WritingGeneration
{
    public partial class Generate
    {
        public class Command : IRequest<Response>
        {
            public static int MinWordCount => 3;

            public static int MaxWordCount => 1000;

            public int WordCount { get; set; }
        }
    }
}
