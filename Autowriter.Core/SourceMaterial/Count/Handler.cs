using MediatR;

namespace Autowriter.Core.Features.SourceMaterial
{
    public partial class Count
    {
        public class Handler : RequestHandler<Query, int>
        {
            private readonly ICountSourceMaterials _sourceCounter;

            public Handler(ICountSourceMaterials sourceReader)
            {
                _sourceCounter = sourceReader;
            }

            protected override int Handle(Query query) =>
                _sourceCounter.CountSources();
        }
    }
}
