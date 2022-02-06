using System.Net.Http;
using Autowriter.Core;
using Autowriter.RazorPages;
using Autowriter.RazorPages.Identity;
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
                        RegisterCoreDb(services, testData);
                        RegisterUserDb(services);
                    }))
                .CreateClient();

        private static void RegisterCoreDb(IServiceCollection services, TestData? testData)
        {
            var connection = new CoreDbConnection("Data Source=:memory:");

            if (testData != null)
            {
                testData.SeedWithTestData(connection);
            }

            services.AddSingleton(connection);
        }

        private static void RegisterUserDb(IServiceCollection services)
        {
            var connection = new UserDbConnection("Data Source=:memory:");
            services.AddSingleton(connection);
        }
    }
}
