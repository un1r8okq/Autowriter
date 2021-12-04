using Dapper;
using System.Data;

namespace Autowriter.Data
{
    public class SourceMaterialRepository : ISourceMaterialRepository
    {
        public class Source
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; } = string.Empty;
        }

        internal class SourceCreationException : Exception { }

        private readonly IDbConnection _connection;

        public SourceMaterialRepository(IDbConnection connection)
        {
            _connection = connection;
            DbHelpers.EnsureDbIsInitialised(connection);
        }

        public const string TableName = "source_material";

        public void CreateSource(DateTime createdDateTime, string content)
        {
            const string query = $"INSERT INTO {TableName} (created, content) VALUES (@created, @content)";
            var parameters = new
            {
                created = createdDateTime,
                content = content,
            };
            var affectedRowCount = _connection.Execute(query, parameters);

            if (affectedRowCount != 1)
            {
                throw new SourceCreationException();
            }
        }

        public Source? GetSource(int id) =>
            _connection
                .Query<Source>($"SELECT id, created, content FROM {TableName} WHERE id = @id", new { id = id })
                .OrderByDescending(model => model.Created)
                .FirstOrDefault();

        public IEnumerable<Source> GetSources() =>
            _connection
                .Query<Source>($"SELECT id, created, content FROM {TableName}")
                .OrderByDescending(model => model.Created);

        public void DeleteSource(int id) =>
            _connection
                .Execute($"DELETE FROM {TableName} WHERE id = @id", new { id });
    }
}
