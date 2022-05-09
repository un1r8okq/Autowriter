namespace Autowriter.Data.Models
{
    public class SourceMaterial
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string Content { get; set; } = string.Empty;
    }
}
