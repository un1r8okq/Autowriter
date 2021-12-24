namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
        public partial class Response
        {
            public bool TextWasEmpty { get; set; }

            public SourceMaterial? CreatedSource { get; set; }

            public class SourceMaterial
            {
                public int Id { get; set; }

                public DateTime Created { get; set; }

                public string Content { get; set; } = string.Empty;
            }
        }
    }
}
