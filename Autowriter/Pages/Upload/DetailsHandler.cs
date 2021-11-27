using Autowriter.Database;
using MediatR;
using System.Data;

namespace Autowriter.Pages.Upload
{
    public class DetailsHandler
    {
        public class Model
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Text { get; set; } = string.Empty;
        }

        public class Query : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class Handler : RequestHandler<Query, Model>
        {
            private readonly IDataRepository _data;

            public Handler(IDataRepository data)
            {
                _data = data;
            }

            protected override Model Handle(Query request)
            {
                var query = "SELECT id, created, text " +
                    $"FROM {TableNames.SourceMaterial} " +
                    "WHERE id = @id";
                var parameters = new { id = request.Id };
                var source = _data
                    .Query<Model>(query, parameters)
                    .OrderByDescending(model => model.Created)
                    .FirstOrDefault();

                return source;
            }
        }
    }
}
