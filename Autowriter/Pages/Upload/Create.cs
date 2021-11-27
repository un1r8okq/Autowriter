using Autowriter.Database;
using MediatR;
using System.Data;

namespace Autowriter.Pages.Upload
{
    public class Create
    {
        public class Model
        {
            public bool TextWasEmpty { get; set; }

            public IEnumerable<Source> Sources { get; set; } = Array.Empty<Source>();

            public class Source
            {
                public DateTime Created { get; set; }

                public string Text { get; set; } = string.Empty;
            }
        }

        public class Command : IRequest<Model>
        {
            public string Text { get; set; } = string.Empty;
        }

        public class Handler : RequestHandler<Command, Model>
        {
            private readonly IDataRepository _data;

            public Handler(IDataRepository data)
            {
                _data = data;
            }

            protected override Model Handle(Command command)
            {
                if (command.Text is null || command.Text == string.Empty)
                {
                    return new Model { TextWasEmpty = true };
                }

                var insertQuery = $"INSERT INTO {TableNames.SourceMaterial} (created, text) VALUES (@created, @text)";
                var parameters = new
                {
                    created = DateTime.UtcNow,
                    text = command.Text,
                };
                _data.Execute(insertQuery, parameters);

                var sources = _data
                    .Query<Model.Source>($"SELECT created, text FROM {TableNames.SourceMaterial}")
                    .OrderByDescending(model => model.Created);

                return new Model
                {
                    TextWasEmpty = false,
                    Sources = sources,
                };
            }
        }
    }
}
