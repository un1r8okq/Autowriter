using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.UI.Tests.IntegrationTests.Pages.Upload
{
    public class WhenISubmitTheUploadPage : GivenAnHttpClient
    {
        public WhenISubmitTheUploadPage(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task TheResponseStatusIsOK()
        {
            var formContent = new Dictionary<string, string>
            {
                { "content", "Integration test content" },
            };
            await GivenTheHttpClientIsAuthenticated();

            await WhenISubmitTheForm("/upload", formContent);

            TheResponseStatusIs(HttpStatusCode.OK);
        }
    }
}
