using Dapper;
using System.Data;

namespace Autowriter.Data
{
    public static class DbHelpers
    {
        public static void EnsureDbIsInitialised(IDbConnection conn)
        {
            EnsureDbConnectionIsOpen(conn);
            EnsureSourceMaterialTableExists(conn);
        }

        private static void EnsureDbConnectionIsOpen(IDbConnection conn)
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
                if (conn.TableDoesNotExist(SourceMaterialRepository.TableName))
                {
                    var query = $"CREATE TABLE {SourceMaterialRepository.TableName} (" +
                        "id INTEGER PRIMARY KEY," +
                        "created TEXT NOT NULL," +
                        "content BLOB NOT NULL" +
                        ");";
                    conn.Execute(query);
                }
            }
        }

        private static bool TableDoesNotExist(this IDbConnection conn, string tableName)
        {
            var query = $"SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{tableName}';";
            var result = conn.QueryFirstOrDefault<string>(query);

            return result == null;
        }
    }
}
