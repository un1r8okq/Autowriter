using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.RazorPages.Tests.IntegrationTests.Pages.Generate
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
            GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/generate");

            TheResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task TheBodyContainsTheTitle()
        {
            GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/generate");

            await TheResponseBodyContains("Generate writing");
        }
    }
}
