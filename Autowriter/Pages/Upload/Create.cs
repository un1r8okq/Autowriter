using Autowriter.Database;
using Dapper;
using MediatR;
using System.Data;

namespace Autowriter.Pages.Upload
{
    public class Create
    {
        public class Model
        {
            public DateTime Created { get; set; }

            public string Text { get; set; } = string.Empty;
        }

        public class Command : IRequest<IEnumerable<Model>>
        {
            public string Text { get; set; } = string.Empty;
        }

        public class Handler : RequestHandler<Command, IEnumerable<Model>>
        {
            private readonly IDbConnection _connection;

            public Handler(IDbConnection connection)
            {
                _connection = connection;
            }

            protected override IEnumerable<Model> Handle(Command command)
            {
                var insertQuery = $"INSERT INTO {TableNames.SourceMaterial} (created, text) VALUES (@created, @text)";
                var parameters = new
                {
                    created = DateTime.UtcNow,
                    text = command.Text,
                };
                _connection.Execute(insertQuery, parameters);
                return _connection
                    .Query<Model>($"SELECT created, text FROM {TableNames.SourceMaterial}")
                    .OrderByDescending(model => model.Created);
            }
        }
    }
}
