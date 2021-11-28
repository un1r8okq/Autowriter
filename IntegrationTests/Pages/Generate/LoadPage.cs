using Autowriter;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Pages.Generate
{
    public class LoadPage : IClassFixture<MockDataWebAppFactory<Startup>>
    {
        private const string Url = "/generate";
        private readonly HttpClient _client;

        public LoadPage(MockDataWebAppFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsOK()
        {
            var response = await _client.GetAsync(Url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task BodyContainsInstructions()
        {
            var response = await _client.GetAsync(Url);
            var body = await response.Content.ReadAsStringAsync();

            Assert.Contains("It's time to get generating!", body);
        }
    }
}