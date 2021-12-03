using Autowriter.Data;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;

namespace IntegrationTests
{
    public class MockDataWebAppFactory<TProgram>: WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.ConfigureTestServices(services =>
            {
                var dbConnection = new SqliteConnection("Data Source=:memory:");
                dbConnection.SeedWithTestData();
                services.AddSingleton<IDbConnection>(dbConnection);
            });
    }

    static class DbTestExtensions
    {
        public static void SeedWithTestData(this IDbConnection dbConnection)
        {
            DbHelpers.EnsureDbIsInitialised(dbConnection);
            InsertSourceMaterial(dbConnection);
        }

        private static void InsertSourceMaterial(IDbConnection dbConnection)
        {
            var query = $"INSERT INTO {SourceMaterialRepository.TableName}" +
                "(created, content) VALUES (@created, @content)";
            var parameters = new
            {
                created = new DateTime(2021, 12, 3, 17, 52, 0),
                content = "Integration test content",
            };
            dbConnection.Execute(query, parameters);
        }
    }
}
