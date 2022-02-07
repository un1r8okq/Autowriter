using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autowriter.RazorPages;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests.Pages.Upload
{
    public class UploadSourceMaterial : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Url = "/upload";
        private readonly HttpClient _client;

        public UploadSourceMaterial(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateTestClient();
        }

        [Fact]
        public async Task ReturnsOK()
        {
            var requestContent = new Dictionary<string, string>
            {
                { "content", "Integration test content" },
            };

            var response = await _client.PostXsrfProtectedForm(Url, requestContent);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
