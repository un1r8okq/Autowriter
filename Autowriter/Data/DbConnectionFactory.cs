using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Autowriter.Data
{
    public static class DbConnectionFactory
    {
        public static IDbConnection Build(string connectionString)
        {
            var conn = new SqliteConnection(connectionString);
            EnsureConnectionIsOpen(conn);
            EnsureTableExists(conn, "source_material");

            return conn;
        }

        private static void EnsureConnectionIsOpen(IDbConnection conn)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }

        private static void EnsureTableExists(IDbConnection conn, string tableName)
        {
            lock (conn)
            {
                var query = $"CREATE TABLE IF NOT EXISTS {tableName} (" +
                    "id INTEGER PRIMARY KEY," +
                    "created TEXT NOT NULL," +
                    "content BLOB NOT NULL" +
                    ");";
                conn.Execute(query);
            }
        }
    }
}
