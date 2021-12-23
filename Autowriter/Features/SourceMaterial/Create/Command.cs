using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Create
    {
        public class Command : IRequest<Response>
        {
            public string Content { get; set; } = string.Empty;
        }
    }
}
