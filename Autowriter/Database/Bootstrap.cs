using Dapper;
using Microsoft.Data.Sqlite;

namespace Autowriter.Database
{
    public interface IDbBootstrapper
    {
        void Bootstrap();
    }

    public class DbBootstrapper : IDisposable, IDbBootstrapper
    {
        private const string UploadsTableName = "uploads";
        private readonly SqliteConnection _connection;

        public DbBootstrapper(IConfiguration config)
        {
            _connection = new SqliteConnection(config.GetSection("DatabaseName").Value);
        }

        public void Bootstrap()
        {
            if (UploadsTableDoesNotExist())
            {
                CreateUploadsTable();
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        private bool UploadsTableDoesNotExist()
        {
            var query = "SELECT name FROM sqlite_master WHERE type='table' AND name = 'uploads';";
            var queryParams = new { tabeName = UploadsTableName };
            var queryResult = _connection.QueryFirstOrDefault<string>(query, queryParams);

            return queryResult == null || queryResult != UploadsTableName;
        }

        private void CreateUploadsTable()
        {
            var query = $"CREATE TABLE {UploadsTableName} (" +
                "id INTEGER PRIMARY KEY," +
                "created TEXT NOT NULL," +
                "text BLOB NOT NULL" +
                ");";
            _connection.Execute(query);
        }
    }
}
