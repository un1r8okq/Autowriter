using AutoMapper;
using Dapper;
using System.Data;

namespace Autowriter.Data
{
    public class SourceMaterial
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string Content { get; set; } = string.Empty;
    }

    public interface ICreateSourceMaterial
    {
        public void CreateSource(DateTime createdDateTime, string content);
    }

    public interface IReadSourceMaterials
    {
        public SourceMaterial? GetSource(int id);

        public IEnumerable<SourceMaterial> GetSources();
    }

    public interface IDeleteSourceMaterial
    {
        public void DeleteSource(int id);
    }

    public class SourceMaterialRepository : ICreateSourceMaterial, IReadSourceMaterials, IDeleteSourceMaterial
    {
        internal class SourceCreationException : Exception { }

        private readonly IDbConnection _connection;
        private readonly IMapper _mapper;

        public SourceMaterialRepository(
            IDbConnection connection,
            IMapper mapper)
        {
            _connection = connection;
            _mapper = mapper;

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

        public SourceMaterial? GetSource(int id) =>
            _connection
                .Query<SourceMaterial>($"SELECT id, created, content FROM {TableName} WHERE id = @id", new { id = id })
                .OrderByDescending(model => model.Created)
                .FirstOrDefault();

        public IEnumerable<SourceMaterial> GetSources() =>
            _connection
                .Query<SourceMaterial>($"SELECT id, created, content FROM {TableName}")
                .OrderByDescending(model => model.Created);

        public void DeleteSource(int id) =>
            _connection
                .Execute($"DELETE FROM {TableName} WHERE id = @id", new { id });
    }
}
