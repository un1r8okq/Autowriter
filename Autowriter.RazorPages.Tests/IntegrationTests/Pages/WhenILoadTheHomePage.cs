using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.RazorPages.Tests.IntegrationTests.Pages
{
    public class WhenILoadTheHomePage : GivenAnHttpClient
    {
        public WhenILoadTheHomePage(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task AndIAmAuthenticated_ResponseStatusIsOK()
        {
            GivenTheHttpClientIsAuthenticated();

            await WhenIGetThePageAt("/");

            TheResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AndIAmAuthenticated_BodyContainsNav()
        {
            GivenTheHttpClientIsAuthenticated();

            await WhenIGetThePageAt("/");

            await TheResponseBodyContains("<nav>");
        }

        [Fact]
        public async Task AndIAmAuthenticated_BodyContainsInstructions()
        {
            GivenTheHttpClientIsAuthenticated();

            await WhenIGetThePageAt("/");

            await TheResponseBodyContains("Welcome to Autowriter 👋🏻");
            await TheResponseBodyContains("<h2>Step one: <a href=\"/upload\">upload some source material</a></h2>");
            await TheResponseBodyContains("<h2>Step two: <a href=\"/generate\">generate some new writing</a>!</h2>");
        }
    }
}
