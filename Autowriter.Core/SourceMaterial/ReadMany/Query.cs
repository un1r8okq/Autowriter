using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class ReadMany
    {
        public class Query : IRequest<IEnumerable<Response>>
        {
        }
    }
}
