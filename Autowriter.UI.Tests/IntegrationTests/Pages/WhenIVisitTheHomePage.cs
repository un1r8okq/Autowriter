using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.UI.Tests.IntegrationTests.Pages
{
    public class WhenIVisitTheHomePage
    {
        private readonly TestHttpClient _theClient;

        public WhenIVisitTheHomePage(WebApplicationFactory<Startup> factory)
        {
            _theClient = new TestHttpClient(factory);
        }

        [Fact]
        public async Task AndIAmNotAuthenticated_IAmRedirectedToTheLoginPage()
        {
            await _theClient.VisitsThePageAt("/");

            _theClient.ResponseStatusIs(HttpStatusCode.Found);
            _theClient.LocationHeaderIs("http://localhost/User/Login?ReturnUrl=%2F");
        }

        [Fact]
        public async Task ResponseStatusIsOK()
        {
            await _theClient.IsAuthenticated();

            await _theClient.VisitsThePageAt("/");

            _theClient.ResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task BodyContainsNav()
        {
            await _theClient.IsAuthenticated();

            await _theClient.VisitsThePageAt("/");

            await _theClient.ResponseBodyContains("<nav>");
        }

        [Fact]
        public async Task BodyContainsInstructions()
        {
            await _theClient.IsAuthenticated();

            await _theClient.VisitsThePageAt("/");

            await _theClient.ResponseBodyContains("Welcome to Autowriter 👋🏻");
            await _theClient.ResponseBodyContains("<h2>Step one: <a href=\"/upload\">upload some source material</a></h2>");
            await _theClient.ResponseBodyContains("<h2>Step two: <a href=\"/generate\">generate some new writing</a>!</h2>");
        }
    }
}
