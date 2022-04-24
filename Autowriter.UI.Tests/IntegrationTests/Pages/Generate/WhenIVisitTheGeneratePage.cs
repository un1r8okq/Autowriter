using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.UI.Tests.IntegrationTests.Pages.Generate
{
    public class WhenIVisitTheGeneratePage : GivenAnHttpClient
    {
        public WhenIVisitTheGeneratePage(WebApplicationFactory<Startup> factory)
        : base(factory)
        {
        }

        [Fact]
        public async Task TheResponseStatusIsOK()
        {
            await GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/generate");

            TheResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task TheBodyContainsTheTitle()
        {
            await GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/generate");

            await TheResponseBodyContains("Generate writing");
        }
    }
}
