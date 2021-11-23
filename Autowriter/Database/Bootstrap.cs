using Dapper;
using Dapper.Contrib.Extensions;
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
            if (UploadsTableDoesNotExist())
            {
                CreateUploadsTable();
                SeedUploadsTable();
            }
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        private bool UploadsTableDoesNotExist()
        {
            var query = "SELECT name FROM sqlite_master WHERE type='table' AND name = 'uploads';";
            var queryParams = new { tabeName = TableNames.Uploads };
            var queryResult = _connection.QueryFirstOrDefault<string>(query, queryParams);

            return queryResult == null || queryResult != TableNames.Uploads;
        }

        private void CreateUploadsTable()
        {
            var query = $"CREATE TABLE {TableNames.Uploads} (" +
                "id INTEGER PRIMARY KEY," +
                "created TEXT NOT NULL," +
                "text BLOB NOT NULL" +
                ");";
            _connection.Execute(query);
        }

        private void SeedUploadsTable()
        {
            var uploads = new Pages.Upload.Index.ViewModel.Upload[]
            {
                new Pages.Upload.Index.ViewModel.Upload
                {
                    Created = new DateTime(1995, 12, 12, 15, 14, 00),
                    Text = "This is a quick story about being born. Wait, how can I write?",
                },
            };
            _connection.Insert(uploads);
        }
    }
}
