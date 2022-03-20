using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.RazorPages.Tests.IntegrationTests.Pages
{
    public class WhenIVisitTheHomePage : GivenAnHttpClient
    {
        public WhenIVisitTheHomePage(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task AndIAmNotAuthenticated_IAmRedirectedToTheLoginPage()
        {
            await WhenIVisitThePageAt("/");

            TheResponseStatusIs(HttpStatusCode.Found);
            TheLocationHeaderIs("http://localhost/User/Login?ReturnUrl=%2F");
        }

        [Fact]
        public async Task ResponseStatusIsOK()
        {
            await GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/");

            TheResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task BodyContainsNav()
        {
            await GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/");

            await TheResponseBodyContains("<nav>");
        }

        [Fact]
        public async Task BodyContainsInstructions()
        {
            await GivenTheHttpClientIsAuthenticated();

            await WhenIVisitThePageAt("/");

            await TheResponseBodyContains("Welcome to Autowriter 👋🏻");
            await TheResponseBodyContains("<h2>Step one: <a href=\"/upload\">upload some source material</a></h2>");
            await TheResponseBodyContains("<h2>Step two: <a href=\"/generate\">generate some new writing</a>!</h2>");
        }
    }
}
