using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Autowriter
{
    public static class DbConnectionFactory
    {
        public static IDbConnection Build(string connectionString)
        {
            var conn = new SqliteConnection(connectionString);
            EnsureConnectionIsOpen(conn);
            EnsureSourceMaterialTableExists(conn);

            return conn;
        }

        private static void EnsureConnectionIsOpen(IDbConnection conn)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }

        private static void EnsureSourceMaterialTableExists(IDbConnection conn)
        {
            lock (conn)
            {
                var query = $"CREATE TABLE IF NOT EXISTS source_material (" +
                    "id INTEGER PRIMARY KEY," +
                    "created TEXT NOT NULL," +
                    "content BLOB NOT NULL" +
                    ");";
                conn.Execute(query);
            }
        }
    }
}
