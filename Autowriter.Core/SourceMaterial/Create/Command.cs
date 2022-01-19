using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Create
    {
        public class Command : IRequest<Response>
        {
            public string Content { get; set; } = string.Empty;
        }
    }
}
