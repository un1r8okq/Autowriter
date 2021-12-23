namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
        public partial class Response
        {
            public bool TextWasEmpty { get; set; }

            public SourceMaterial? CreatedSource { get; set; }
        }
    }
}
