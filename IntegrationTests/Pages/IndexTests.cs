using Autowriter;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Pages
{
    public class IndexTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Url = "/";
        private readonly HttpClient _client;

        public IndexTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateTestClient();
        }

        [Fact]
        public async Task ReturnsOK()
        {
            var response = await _client.GetAsync(Url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task BodyContainsNav()
        {
            var response = await _client.GetAsync(Url);
            var body = await response.Content.ReadAsStringAsync();

            Assert.Contains("<nav>", body);
        }

        [Fact]
        public async Task BodyContainsWelcomeMessage()
        {
            var response = await _client.GetAsync(Url);
            var body = await response.Content.ReadAsStringAsync();

            Assert.Contains("Welcome to Autowriter 👋🏻", body);
            Assert.Contains("<h2>Step one: <a href=\"/upload\">upload some source material</a></h2>", body);
            Assert.Contains("<h2>Step two: <a href=\"/generate\">generate some new writing</a>!</h2>", body);
        }
    }
}
