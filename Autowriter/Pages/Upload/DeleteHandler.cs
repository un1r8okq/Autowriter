using Autowriter.Database;
using MediatR;

namespace Autowriter.Pages.Upload
{
    public class DeleteHandler
    {
        public class Command : IRequest<Response>
        {
            public int Id { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Text { get; set; } = string.Empty;

            public string Error { get; set; } = string.Empty;
        }

        public class Handler : RequestHandler<Command, Response>
        {
            private readonly IDataRepository _data;

            public Handler(IDataRepository data)
            {
                _data = data;
            }

            protected override Response Handle(Command command)
            {
                var query = $"DELETE FROM {TableNames.SourceMaterial} " +
                    "WHERE id = @id";
                var parameters = new { id = command.Id };
                var affectedRowCount = _data.Execute(query, parameters);

                if (affectedRowCount != 1)
                {
                    var response = _data
                        .Query<Response>($"SELECT id, created, text FROM {TableNames.SourceMaterial}")
                        .OrderByDescending(model => model.Created)
                        .First();
                    response.Error = "Failed to delete source material";
                    return response;
                }

                return new Response { Error = "" };
            }
        }
    }
}
