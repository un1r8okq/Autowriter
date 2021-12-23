using System;
using System.Collections.Generic;
using System.Data;
using Autowriter.Data;
using Dapper;

namespace IntegrationTests
{
    public class TestData
    {
        public IEnumerable<SourceMaterial> Sources { get; set; } = Array.Empty<SourceMaterial>();

        public void SeedWithTestData(IDbConnection dbConnection)
        {
            DbHelpers.EnsureDbIsInitialised(dbConnection);

            foreach (var source in Sources)
            {
                InsertSourceMaterial(dbConnection, source);
            }
        }

        private static void InsertSourceMaterial(IDbConnection dbConnection, SourceMaterial source)
        {
            var query = $"INSERT INTO {DbHelpers.SourceMaterialTableName}" +
                "(created, content) VALUES (@created, @content)";
            var parameters = new
            {
                created = source.Created,
                content = source.Content,
            };
            dbConnection.Execute(query, parameters);
        }

        public class SourceMaterial
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; } = string.Empty;
        }
    }
}
