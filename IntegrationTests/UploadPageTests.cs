using Microsoft.AspNetCore.Mvc.Testing;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class UploadPageTests
    {
        private const string Url = "/upload";
        private readonly WebApplicationFactory<Program> _app = new();

        [Fact]
        public async Task Get_Index_ReturnsOK()
        {
            var client = _app.CreateClient();

            var response = await client.GetAsync(Url);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Get_Index_BodyContainsText()
        {
            var client = _app.CreateClient();

            var response = await client.GetAsync(Url);
            var body = await response.Content.ReadAsStringAsync();

            Assert.Contains("Before you can generate writing, you will need to upload some source material!", body);
        }

        [Fact]
        public async Task Get_Index_LatencyLessThan10Milliseconds()
        {
            var client = _app.CreateClient();
            var stopwatch = new Stopwatch();
            
            stopwatch.Start();
            await client.GetAsync(Url);
            stopwatch.Stop();

            Assert.InRange(stopwatch.ElapsedMilliseconds, 0, 10);
        }
    }
}
