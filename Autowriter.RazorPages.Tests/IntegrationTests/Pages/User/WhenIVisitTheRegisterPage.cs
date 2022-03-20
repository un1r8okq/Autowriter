using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.RazorPages.Tests.IntegrationTests.Pages.User
{
    public class WhenIVisitTheRegisterPage : GivenAnHttpClient
    {
        public WhenIVisitTheRegisterPage(WebApplicationFactory<Startup> factory)
        : base(factory)
        {
        }

        [Fact]
        public async Task TheResponseStatusIsOK()
        {
            await WhenIVisitThePageAt("/user/register");

            TheResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task TheBodyContainsTheRegisterButton()
        {
            await WhenIVisitThePageAt("/user/register");

            await TheResponseBodyContains("<input type=\"submit\" value=\"Register\" />");
        }
    }
}
