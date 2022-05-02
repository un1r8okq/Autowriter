using System;
using System.Collections.Generic;
using System.Linq;
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
    public abstract class GivenAnHttpClient : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly string _testEmail;
        private readonly string _testPassword;

        private HttpResponseMessage? _httpResponseMessage;
        private string? _responseBody;
        private readonly HttpClient? _httpClient;
        private readonly WebApplicationFactory<Startup> _factory;

        public GivenAnHttpClient(WebApplicationFactory<Startup> factory)
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
            _testEmail = $"{Guid.NewGuid()}@example.com";
            _testPassword = Guid.NewGuid().ToString();
        }

        public async Task GivenTheHttpClientIsAuthenticated()
        {
            await RegisterTestUser();
            await AuthenticateAsTestUser();
        }

        public async Task WhenIVisitThePageAt(string url)
        {
            _httpResponseMessage = await _httpClient!.GetAsync(url);
        }

        public async Task WhenISubmitTheForm(string url, Dictionary<string, string> formBody)
        {
            _httpResponseMessage = await PostXsrfProtectedForm(url, formBody);
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

        private static void RegisterCoreDb(IServiceCollection services)
        {
            services.Remove(services.Single(s => s.ServiceType == typeof(CoreDbConnection)));
            services.AddSingleton(_ => new CoreDbConnection("Data Source=:memory:"));
        }

        private static void RegisterUserDb(IServiceCollection services)
        {
            services.Remove(services.Single(s => s.ServiceType == typeof(UserDbConnection)));
            services.AddScoped(_ => new UserDbConnection("Data Source=TestUserDb.sqlite"));
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
                    { "email", _testEmail },
                    { "password", _testPassword },
                };
            await PostXsrfProtectedForm("/User/Register", formContent);
        }

        private async Task AuthenticateAsTestUser()
        {
            var formContent = new Dictionary<string, string>
                {
                    { "email", _testEmail },
                    { "password", _testPassword },
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
