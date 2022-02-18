using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.RazorPages.Tests.IntegrationTests.Pages.Upload
{
    public class LoadPage : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Url = "/upload";
        private readonly HttpClient _client;

        public LoadPage(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateAuthenticatedTestClient();
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
        public async Task CanUploadSourceContent()
        {
            var requestBody = new Dictionary<string, string>
            {
                { "content", "Integration test content" },
            };

            var response = await _client.PostXsrfProtectedForm(Url, requestBody);
            await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}