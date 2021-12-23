using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Count
    {
        public class Query : IRequest<int>
        {
        }
    }
}
