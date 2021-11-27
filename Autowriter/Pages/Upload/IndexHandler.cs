using Autowriter.Database;
using MediatR;
using System.Data;

namespace Autowriter.Pages.Upload
{
    public class IndexHandler
    {
        public class Model
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Text { get; set; } = string.Empty;
        }

        public class Query : IRequest<IEnumerable<Model>> { }

        public class Handler : RequestHandler<Query, IEnumerable<Model>>
        {
            private readonly IDataRepository _data;

            public Handler(IDataRepository data)
            {
                _data = data;
            }

            protected override IEnumerable<Model> Handle(Query request) =>
                _data
                    .Query<Model>($"SELECT id, created, text FROM {TableNames.SourceMaterial}")
                    .OrderByDescending(model => model.Created);
        }
    }
}
