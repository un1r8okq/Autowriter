using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public partial class ReadSingle
    {
        public class Query : IRequest<Response>
        {
            public int Id { get; set; }
        }
    }
}
