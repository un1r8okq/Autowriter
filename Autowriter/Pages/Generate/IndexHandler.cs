using Autowriter.Database;
using MediatR;

namespace Autowriter.Pages.Generate
{
    public class IndexHandler
    {
        public class Model
        {
            public int SourceCount { get; set; }
        }

        public class Query : IRequest<Model> { }

        public class Handler : RequestHandler<Query, Model>
        {
            private readonly IDataRepository _data;

            public Handler(IDataRepository data)
            {
                _data = data;
            }

            protected override Model Handle(Query request)
            {
                var sourceCount  = _data
                    .Query<int>($"SELECT count(*) FROM {TableNames.SourceMaterial}")
                    .First();

                return new Model { SourceCount = sourceCount };
            }
        }
    }
}
