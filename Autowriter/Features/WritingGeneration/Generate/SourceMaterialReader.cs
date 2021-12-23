using System.Data;
using Autowriter.Data;
using Dapper;

namespace Autowriter.Features.WritingGeneration.Generate
{
    public interface IReadSourceMaterial
    {
        IEnumerable<string> GetSources();
    }

    public class SourceMaterialReader : IReadSourceMaterial
    {
        private readonly IDbConnection _connection;

        public SourceMaterialReader(IDbConnection connection)
        {
            _connection = connection;

            DbHelpers.EnsureDbIsInitialised(connection);
        }

        public IEnumerable<string> GetSources() =>
            _connection.Query<string>(
                $"SELECT content FROM {DbHelpers.SourceMaterialTableName}");
    }
}