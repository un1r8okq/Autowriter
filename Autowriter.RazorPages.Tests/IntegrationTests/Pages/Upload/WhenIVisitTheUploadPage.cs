using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.RazorPages.Tests.IntegrationTests.Pages.Upload
{
    public class WhenIVisitTheUploadPage : GivenAnHttpClient
    {
        public WhenIVisitTheUploadPage(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task TheResponseStatusIsOK()
        {
            GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/upload");

            TheResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task TheBodyContainsInstructions()
        {
            GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/upload");

            await TheResponseBodyContains("Before you can generate writing, you will need to upload some source material!");
        }
    }
}
