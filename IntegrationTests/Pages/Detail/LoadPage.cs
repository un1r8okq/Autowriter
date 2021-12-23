using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Autowriter;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests.Pages.Detail
{
    public class LoadPage : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string Url = "/upload/details?id=1";
        private readonly HttpClient _client;

        public LoadPage(WebApplicationFactory<Startup> factory)
        {
            var testData = new TestData
            {
                Sources = new[]
                {
                    new TestData.SourceMaterial
                    {
                        Created = new DateTime(2021, 12, 3, 17, 52, 0),
                        Content = "Integration test",
                    },
                },
            };
            _client = factory.CreateTestClient(testData);
        }

        [Fact]
        public async Task ReturnsOK()
        {
            var response = await _client.GetAsync(Url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task BodyContainsSourceMaterialText()
        {
            var response = await _client.GetAsync(Url);
            var body = await response.Content.ReadAsStringAsync();

            Assert.Contains("Uploaded on Saturday, 4 December 2021 at 6:52 am", body);
        }
    }
}
