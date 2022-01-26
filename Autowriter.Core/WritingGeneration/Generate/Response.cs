namespace Autowriter.Core.Features.WritingGeneration
{
    public partial class Generate
    {
        public class Response
        {
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
