namespace Autowriter.Features.SourceMaterial
{
    public partial class ReadMany
    {
        public class Response
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; } = string.Empty;
        }
    }
}
