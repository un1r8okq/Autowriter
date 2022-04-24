using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.UI.Tests.IntegrationTests.Pages.Detail
{
    public class WhenIVisitTheUploadDetailPage : GivenAnHttpClient
    {
        private readonly Dictionary<string, string> _testFormContent = new ()
        {
            { "content", "Integration test content" },
        };

        public WhenIVisitTheUploadDetailPage(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task TheResponseStatusIsOK()
        {
            await GivenTheHttpClientIsAuthenticated();
            await WhenISubmitTheForm("/upload", _testFormContent);

            await WhenIVisitThePageAt("/upload/details?id=1");

            TheResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task BodyContainsSourceMaterialText()
        {
            await GivenTheHttpClientIsAuthenticated();
            await WhenISubmitTheForm("/upload", _testFormContent);

            await WhenIVisitThePageAt("/upload/details?id=1");

            await TheResponseBodyContains($"Uploaded on {DateTime.Now:D} at {DateTime.Now:t}");
        }
    }
}
