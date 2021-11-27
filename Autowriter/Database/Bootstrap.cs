using Dapper;
using System.Data;

namespace Autowriter.Database
{
    public interface IDbBootstrapper
    {
        void Bootstrap();
    }

    public class DbBootstrapper : IDisposable, IDbBootstrapper
    {
        private readonly IDbConnection _connection;

        public DbBootstrapper(IDbConnection connection)
        {
            _connection = connection;
        }

        public void Bootstrap()
        {
            lock (_connection)
            {
                if (UploadsTableDoesNotExist())
                {
                    CreateUploadsTable();
                }
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        private bool UploadsTableDoesNotExist()
        {
            var query = $"SELECT name FROM sqlite_master WHERE type='table' AND name = '{TableNames.SourceMaterial}';";
            var parameters = new { tabeName = TableNames.SourceMaterial };
            var result = _connection.QueryFirstOrDefault<string>(query, parameters);

            return result == null || result != TableNames.SourceMaterial;
        }

        private void CreateUploadsTable()
        {
            var query = $"CREATE TABLE {TableNames.SourceMaterial} (" +
                "id INTEGER PRIMARY KEY," +
                "created TEXT NOT NULL," +
                "text BLOB NOT NULL" +
                ");";
            _connection.Execute(query);
        }
    }
}
