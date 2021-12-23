using System.Data;
using Autowriter.Data;
using Dapper;

namespace Autowriter.Features.SourceMaterial
{
    public interface IReadSourceMaterials
    {
        public SourceMaterial? GetSource(int id);

        public IEnumerable<SourceMaterial> GetSources();
    }

    public interface IDeleteSourceMaterial
    {
        public void DeleteSource(int id);
    }

    public class SourceMaterial
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public string Content { get; set; } = string.Empty;
    }

    public class SourceMaterialRepository : IReadSourceMaterials, IDeleteSourceMaterial
    {
        public const string TableName = "source_material";

        private readonly IDbConnection _connection;

        public SourceMaterialRepository(IDbConnection connection)
        {
            _connection = connection;

            DbHelpers.EnsureDbIsInitialised(connection);
        }

        public SourceMaterial? GetSource(int id) =>
            _connection
                .Query<SourceMaterial>($"SELECT id, created, content FROM {TableName} WHERE id = @id", new { id })
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
