using Autowriter.Database;
using Dapper;
using MediatR;
using System.Data;

namespace Autowriter.Pages.Upload
{
    public class Index
    {
        public class Model
        {
            public DateTime Created { get; set; }

            public string Text { get; set; } = string.Empty;
        }

        public class Query : IRequest<IEnumerable<Model>> { }

        public class Handler : RequestHandler<Query, IEnumerable<Model>>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            protected override IEnumerable<Model> Handle(Query request) =>
                _connection
                    .Query<Model>($"SELECT created, text FROM {TableNames.Uploads}")
                    .OrderByDescending(model => model.Created);
        }
    }
}
