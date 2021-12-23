using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public partial class ReadMany
    {
        public class Query : IRequest<IEnumerable<Response>> { }
    }
}
