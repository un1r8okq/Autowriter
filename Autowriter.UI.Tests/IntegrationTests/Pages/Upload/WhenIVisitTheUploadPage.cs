using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.UI.Tests.IntegrationTests.Pages.Upload
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
            await GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/upload");

            TheResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task TheBodyContainsInstructions()
        {
            await GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/upload");

            await TheResponseBodyContains("Before you can generate writing, you will need to upload some source material!");
        }
    }
}
