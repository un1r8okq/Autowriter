using Autowriter.Data;
using Dapper;
using System.Data;

namespace Autowriter.Pages.Generate
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
