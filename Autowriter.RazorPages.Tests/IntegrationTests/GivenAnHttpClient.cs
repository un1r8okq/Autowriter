using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autowriter.Core;
using Autowriter.RazorPages.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Autowriter.RazorPages.Tests.IntegrationTests
{
    public abstract class GivenAnHttpClient : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string TestEmail = "test-user@example.com";
        private const string TestPassword = "100% secure passphrase - for tests!";

        private HttpResponseMessage? _httpResponseMessage;
        private string? _responseBody;
        private readonly HttpClient? _httpClient;
        private readonly WebApplicationFactory<Startup> _factory;

        public GivenAnHttpClient(WebApplicationFactory<Startup> factory)
        {
            TestData? testData = null;
            _factory = factory;
            _httpClient = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        RegisterCoreDb(services, testData);
                        RegisterUserDb(services);
                    }))
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });
        }

        public void GivenTheHttpClientIsAuthenticated()
        {
            RegisterTestUser(_httpClient!);
            AuthenticateAsTestUser(_httpClient!);
        }

        public async Task WhenIVisitThePageAt(string url)
        {
            _httpResponseMessage = await _httpClient!.GetAsync(url);
        }

        public async Task WhenISubmitTheForm(string url, Dictionary<string, string> keyValuePairs)
        {
            _httpResponseMessage = await _httpClient!.PostXsrfProtectedForm(url, keyValuePairs);
        }

        public void TheResponseStatusIs(HttpStatusCode statusCode)
        {
            Assert.Equal(statusCode, _httpResponseMessage!.StatusCode);
        }

        public async Task TheResponseBodyContains(string str)
        {
            Assert.Contains(str, await ResponseBody());
        }

        public void TheLocationHeaderIs(string expectedValue)
        {
            Assert.Equal(expectedValue, _httpResponseMessage!.Headers.Location!.ToString());
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

        private async Task<string> ResponseBody()
        {
            if (_responseBody == null)
            {
                _responseBody = await _httpResponseMessage!.Content.ReadAsStringAsync();
            }

            return _responseBody;
        }

        private static void RegisterTestUser(HttpClient client)
        {
            var formContent = new Dictionary<string, string>
                {
                    { "email", TestEmail },
                    { "password", TestPassword },
                };
            var responseTask = client.PostXsrfProtectedForm("/User/Register", formContent);
            responseTask.Wait();
        }

        private static void AuthenticateAsTestUser(HttpClient client)
        {
            var formContent = new Dictionary<string, string>
                {
                    { "email", TestEmail },
                    { "password", TestPassword },
                };
            var responseTask = client.PostXsrfProtectedForm("/User/Login", formContent);
            responseTask.Wait();
        }
    }
}
