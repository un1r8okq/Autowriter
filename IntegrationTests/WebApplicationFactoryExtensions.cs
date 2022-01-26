using System.Data;
using System.Net.Http;
using Autowriter.Core;
using Autowriter.RazorPages;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests
{
    public static class WebApplicationFactoryExtensions
    {
        public static HttpClient CreateTestClient(this WebApplicationFactory<Startup> factory, TestData? testData = null) =>
            factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        var dbConnection = DbConnectionFactory.Build("Data Source=:memory:");

                        if (testData != null)
                        {
                            testData.SeedWithTestData(dbConnection);
                        }

                        services.AddSingleton<IDbConnection>(dbConnection);
                    }))
                .CreateClient();
    }
}
