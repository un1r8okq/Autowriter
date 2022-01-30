using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Autowriter.RazorPages.Identity
{
    public static class Data
    {
        public static string UsersTableName => "users";

        public static IDbConnection BuildIdentityDbConnection(string connectionString)
        {
            var conn = new SqliteConnection(connectionString);
            EnsureConnectionIsOpen(conn);
            EnsureUserTableExists(conn);

            return conn;
        }

        private static void EnsureConnectionIsOpen(IDbConnection conn)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }

        private static void EnsureUserTableExists(IDbConnection conn)
        {
            lock (conn)
            {
                var query = $"CREATE TABLE IF NOT EXISTS {UsersTableName} (" +
                    "id INTEGER PRIMARY KEY, " +
                    "concurrencyStamp TEXT, " +
                    "normalizedUserName TEXT UNIQUE NOT NULL, " +
                    "passwordHash TEXT, " +
                    "securityStamp TEXT, " +
                    "userName TEXT UNIQUE NOT NULL);";
                conn.Execute(query);
            }
        }
    }
}
