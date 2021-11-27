using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Pages.Upload
{
    public class IndexTests
    {
        private const string Url = "/upload";
        private readonly HttpClient _client;

        public IndexTests()
        {
            var app = new WebApplicationFactory<Program>();
            _client = app.CreateClient();
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

            Assert.Contains("Before you can generate writing, you will need to upload some source material!", body);
        }

        [Fact]
        public async Task LatencyLessThan10Milliseconds()
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            await _client.GetAsync(Url);
            stopwatch.Stop();

            Assert.InRange(stopwatch.ElapsedMilliseconds, 0, 10);
        }
    }
}