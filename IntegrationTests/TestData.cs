using Autowriter.Data;
using Dapper;
using System;
using System.Data;

namespace IntegrationTests
{
    public static class TestData
    {
        public static void SeedDb(IDbConnection conn)
        {
            DbHelpers.EnsureDbIsInitialised(conn);

            var query = $"INSERT INTO {SourceMaterialRepository.TableName}" +
                "(created, content) VALUES (@created, @content)";
            var parameters = new
            {
                created = new DateTime(2021, 12, 3, 17, 52, 0),
                content = "Integration test content",
            };
            conn.Execute(query, parameters);
        }
    }
}
