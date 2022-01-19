using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Count
    {
        public class Query : IRequest<int>
        {
        }
    }
}
