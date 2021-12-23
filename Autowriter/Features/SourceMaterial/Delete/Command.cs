using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }
    }
}
