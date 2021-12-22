using Autowriter.Data;
using Dapper;
using System.Data;

namespace Autowriter.Pages.Generate
{
    public interface IReadSourceMaterial
    {
        IEnumerable<GenerateHandler.SourceMaterial> GetSources();
    }

    public class SourceMaterialReader : IReadSourceMaterial
    {
        private readonly IDbConnection _connection;

        public SourceMaterialReader(IDbConnection connection)
        {
            _connection = connection;

            DbHelpers.EnsureDbIsInitialised(connection);
        }

        public IEnumerable<GenerateHandler.SourceMaterial> GetSources() =>
            _connection.Query<GenerateHandler.SourceMaterial>(
                $"SELECT id, created, content FROM {DbHelpers.SourceMaterialTableName}");
    }
}
