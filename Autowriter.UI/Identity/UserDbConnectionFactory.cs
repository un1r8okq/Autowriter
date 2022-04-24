using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Autowriter.UI.Identity
{
    public class UserDbConnection : IDbConnection
    {
        private readonly SqliteConnection _conn;

        public UserDbConnection(string connectionString)
        {
            _conn = new SqliteConnection(connectionString);

            if (_conn is null)
            {
                throw new Exception($"Unable to create {typeof(UserDbConnection)}");
            }

            EnsureConnectionIsOpen();
            EnsureUserTableExists();
        }

        public static string UsersTableName => "users";

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

        private void EnsureUserTableExists()
        {
            lock (_conn)
            {
                var query = $"CREATE TABLE IF NOT EXISTS {UsersTableName} (" +
                    "id INTEGER PRIMARY KEY, " +
                    "concurrencyStamp TEXT, " +
                    "normalizedUserName TEXT UNIQUE NOT NULL, " +
                    "passwordHash TEXT, " +
                    "securityStamp TEXT, " +
                    "userName TEXT UNIQUE NOT NULL);";
                _conn.Execute(query);
            }
        }
    }
}
