using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.RazorPages.Tests.IntegrationTests.Pages.User
{
    public class Register : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Url = "/user/register";
        private readonly HttpClient _client;

        public Register(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateTestClient();
        }

        [Fact]
        public async Task WhenNotAuthenticated_CanLoadPage()
        {
            var response = await _client.GetAsync(Url);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("<input type=\"submit\" value=\"Register\" />", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
