using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class LandingPageTests
    {
        private const string Url = "/";
        private readonly HttpClient _client;

        public LandingPageTests()
        {
            var app = new WebApplicationFactory<Program>();
            _client = app.CreateClient();
        }

        [Fact]
        public async Task Get_Index_ReturnsOK()
        {
            var response = await _client.GetAsync(Url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_Index_BodyContainsText()
        {
            var response = await _client.GetAsync(Url);
            var body = await response.Content.ReadAsStringAsync();

            Assert.Contains("Welcome to Autowriter 👋🏻", body);
        }

        [Fact]
        public async Task Get_Index_LatencyLessThan10Milliseconds()
        {
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            await _client.GetAsync(Url);
            stopwatch.Stop();

            Assert.InRange(stopwatch.ElapsedMilliseconds, 0, 10);
        }
    }
}
