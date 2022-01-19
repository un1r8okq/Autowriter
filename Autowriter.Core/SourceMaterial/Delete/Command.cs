using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }
    }
}
