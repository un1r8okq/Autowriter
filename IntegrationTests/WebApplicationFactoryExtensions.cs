using System.Collections.Generic;
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
        private const string Email = "test-user@example.com";
        private const string Password = "100% secure passphrase - for tests!";

        public static HttpClient CreateAuthenticatedTestClient(this WebApplicationFactory<Startup> factory, TestData? testData = null)
        {
            var client = CreateTestClient(factory, testData);
            RegisterTestUser(client);
            AuthenticateAsTestUser(client);

            return client;
        }

        public static HttpClient CreateTestClient(this WebApplicationFactory<Startup> factory, TestData? testData = null) =>
            factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        RegisterCoreDb(services, testData);
                        RegisterUserDb(services);
                    }))
                .CreateClient();

        private static void RegisterTestUser(HttpClient client)
        {
            var formContent = new Dictionary<string, string>
                {
                    { "email", Email },
                    { "password", Password },
                };
            var responseTask = client.PostXsrfProtectedForm("/User/Register", formContent);
            responseTask.Wait();
            responseTask.Result.EnsureSuccessStatusCode();
        }

        private static void AuthenticateAsTestUser(HttpClient client)
        {
            var formContent = new Dictionary<string, string>
                {
                    { "email", Email },
                    { "password", Password },
                };
            var responseTask = client.PostXsrfProtectedForm("/User/Login", formContent);
            responseTask.Wait();
            responseTask.Result.EnsureSuccessStatusCode();
        }

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
