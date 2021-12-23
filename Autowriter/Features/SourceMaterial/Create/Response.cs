namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
        public class Response
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
