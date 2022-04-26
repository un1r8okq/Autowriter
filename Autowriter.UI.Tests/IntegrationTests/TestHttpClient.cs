using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autowriter.Core;
using Autowriter.UI.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Autowriter.UI.Tests.IntegrationTests
{
    public class TestHttpClient : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string TestEmail = "test-user@example.com";
        private const string TestPassword = "100% secure passphrase - for tests!";

        private HttpResponseMessage? _httpResponseMessage;
        private string? _responseBody;
        private readonly HttpClient? _httpClient;
        private readonly WebApplicationFactory<Startup> _factory;

        public TestHttpClient(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _httpClient = _factory
                .WithWebHostBuilder(builder =>
                    builder.ConfigureTestServices(services =>
                    {
                        RegisterCoreDb(services);
                        RegisterUserDb(services);
                    }))
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });
        }

        public async Task IsAuthenticated()
        {
            await RegisterTestUser();
            await AuthenticateAsTestUser();
        }

        public async Task VisitsThePageAt(string url)
        {
            _httpResponseMessage = await _httpClient!.GetAsync(url);
        }

        public async Task WhenISubmitTheForm(string url, Dictionary<string, string> formBody)
        {
            _httpResponseMessage = await PostXsrfProtectedForm(url, formBody);
        }

        public void ResponseStatusIs(HttpStatusCode statusCode)
        {
            Assert.Equal(statusCode, _httpResponseMessage!.StatusCode);
        }

        public async Task ResponseBodyContains(string str)
        {
            Assert.Contains(str, await ResponseBody());
        }

        public void LocationHeaderIs(string expectedValue)
        {
            Assert.Equal(expectedValue, _httpResponseMessage!.Headers.Location!.ToString());
        }

        private static void RegisterCoreDb(IServiceCollection services)
        {
            var connection = new CoreDbConnection("Data Source=:memory:");

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

        private async Task RegisterTestUser()
        {
            var formContent = new Dictionary<string, string>
                {
                    { "email", TestEmail },
                    { "password", TestPassword },
                };
            await PostXsrfProtectedForm("/User/Register", formContent);
        }

        private async Task AuthenticateAsTestUser()
        {
            var formContent = new Dictionary<string, string>
                {
                    { "email", TestEmail },
                    { "password", TestPassword },
                };
            await PostXsrfProtectedForm("/User/Login", formContent);
        }

        private async Task<HttpResponseMessage> PostXsrfProtectedForm(string url, Dictionary<string, string> formElements)
        {
            var xsrfToken = await GetXsrfToken(url);
            var body = new Dictionary<string, string>(formElements)
            {
                { "__RequestVerificationToken", xsrfToken },
            };
            var content = new FormUrlEncodedContent(body);
            var response = await _httpClient!.PostAsync(url, content);

            return response;
        }

        private async Task<string> GetXsrfToken(string url)
        {
            var pageResponse = await _httpClient!.GetAsync(url);
            var pageBody = await pageResponse.Content.ReadAsStringAsync();
            var xsrfTokenRegex = new Regex("<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(?<token>.+)\"");
            var xsrfToken = xsrfTokenRegex.Match(pageBody).Groups[1].Value;
            return xsrfToken;
        }
    }
}
