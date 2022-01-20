namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadSingle
    {
        public class Response
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; } = string.Empty;
        }
    }
}
