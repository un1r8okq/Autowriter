using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Autowriter.Core
{
    public class CoreDbConnection : IDbConnection
    {
        private readonly SqliteConnection _conn;

        public CoreDbConnection(string connectionString)
        {
            _conn = new SqliteConnection(connectionString);

            if (_conn is null)
            {
                throw new Exception($"Unable to create {typeof(CoreDbConnection)}");
            }

            EnsureConnectionIsOpen();
            EnsureSourceMaterialTableExists();
        }

        public string ConnectionString
        {
            get => _conn.ConnectionString;

            // Nullability of reference types in type of parameter doesn't match implicitly
            // implemented member (possibly because of nullability attributes).
            #pragma warning disable CS8767
            set => _conn.ConnectionString = value ?? string.Empty;
            #pragma warning restore CS8767
        }

        public int ConnectionTimeout => _conn.ConnectionTimeout;

        public string Database => _conn.Database;

        public ConnectionState State => _conn.State;

        public IDbTransaction BeginTransaction() => _conn.BeginTransaction();

        public IDbTransaction BeginTransaction(IsolationLevel il) => _conn.BeginTransaction(il);

        public void ChangeDatabase(string databaseName) => _conn.ChangeDatabase(databaseName);

        public void Close() => _conn.Close();

        public IDbCommand CreateCommand() => _conn.CreateCommand();

        public void Dispose()
        {
            _conn.Dispose();
            GC.SuppressFinalize(this);
        }

        public void Open() => _conn.Open();

        private void EnsureConnectionIsOpen()
        {
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
        }

        private void EnsureSourceMaterialTableExists()
        {
            lock (_conn)
            {
                var query = $"CREATE TABLE IF NOT EXISTS source_material (" +
                    "id INTEGER PRIMARY KEY," +
                    "created TEXT NOT NULL," +
                    "content BLOB NOT NULL" +
                    ");";
                _conn.Execute(query);
            }
        }
    }
}
