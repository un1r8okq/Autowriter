using MediatR;

namespace Autowriter.Features.SourceMaterial
{
    public partial class Count
    {
        public class Handler : RequestHandler<Query, int>
        {
            private readonly IReadSourceMaterials _sourceReader;

            public Handler(IReadSourceMaterials sourceReader)
            {
                _sourceReader = sourceReader;
            }

            protected override int Handle(Query query) =>
                _sourceReader.GetSources().Count();
        }
    }
}
