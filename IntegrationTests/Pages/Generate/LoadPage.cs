using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autowriter;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests.Pages.Generate
{
    public class LoadPage : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Url = "/generate";
        private readonly HttpClient _client;

        public LoadPage(WebApplicationFactory<Startup> factory)
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
        public async Task BodyContainsTitle()
        {
            var response = await _client.GetAsync(Url);
            var body = await response.Content.ReadAsStringAsync();

            Assert.Contains("Generate writing", body);
        }
    }
}
