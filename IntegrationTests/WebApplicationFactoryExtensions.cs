using Autowriter;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Net.Http;

namespace IntegrationTests
{
    public static class WebApplicationFactoryExtensions
    {
        public static HttpClient CreateTestClient(this WebApplicationFactory<Startup> factory, TestData? testData = null) =>
            factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        var dbConnection = new SqliteConnection("Data Source=:memory:");

                        if (testData != null)
                        {
                            testData.SeedWithTestData(dbConnection);
                        }
                        
                        services.AddSingleton<IDbConnection>(dbConnection);
                    }))
                .CreateClient();
    }
}
