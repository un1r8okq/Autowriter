using Autowriter;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                await GetXsrfTokenKeypair(),
                new KeyValuePair<string, string>("content", "Integration test content"),
            });

            var response = await _client.PostAsync(Url, content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private async Task<KeyValuePair<string, string>> GetXsrfTokenKeypair()
        {
            var pageResponse = await _client.GetAsync(Url);
            var pageBody = await pageResponse.Content.ReadAsStringAsync();
            var xsrfTokenRegex = new Regex("<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(?<token>.+)\"");
            var xsrfToken = xsrfTokenRegex.Match(pageBody).Groups[1].Value;
            return new KeyValuePair<string, string>("__RequestVerificationToken", xsrfToken);
        }
    }
}
