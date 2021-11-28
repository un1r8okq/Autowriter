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

        private const string TableName = "source_material";
        private readonly IDbConnection _connection;

        public SourceMaterialRepository(IDbConnection connection)
        {
            _connection = connection;

            lock (_connection)
            {
                if (SourceMaterialTableDoesNotExist())
                {
                    CreateSourceMaterialTable();
                }
            }
        }

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

        public Source GetSource(int id) =>
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

        private bool SourceMaterialTableDoesNotExist()
        {
            var query = $"SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{TableName}';";
            var result = _connection.QueryFirstOrDefault<string>(query);

            return result == null;
        }

        private void CreateSourceMaterialTable()
        {
            var query = $"CREATE TABLE {TableName} (" +
                "id INTEGER PRIMARY KEY," +
                "created TEXT NOT NULL," +
                "content BLOB NOT NULL" +
                ");";
            _connection.Execute(query);
        }
    }
}
