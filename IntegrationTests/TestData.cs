using Autowriter.Data;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace IntegrationTests
{
    public class TestData
    {
        public class SourceMaterial
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; }
        }

        public IEnumerable<SourceMaterial> Sources { get; set; } = Array.Empty<SourceMaterial>();

        public void SeedWithTestData(IDbConnection dbConnection)
        {
            DbHelpers.EnsureDbIsInitialised(dbConnection);

            foreach (var source in Sources)
            {
                InsertSourceMaterial(dbConnection, source);
            }
        }

        private void InsertSourceMaterial(IDbConnection dbConnection, SourceMaterial source)
        {
            var query = $"INSERT INTO {SourceMaterialRepository.TableName}" +
                "(created, content) VALUES (@created, @content)";
            var parameters = new
            {
                created = source.Created,
                content = source.Content,
            };
            dbConnection.Execute(query, parameters);
        }
    }
}
