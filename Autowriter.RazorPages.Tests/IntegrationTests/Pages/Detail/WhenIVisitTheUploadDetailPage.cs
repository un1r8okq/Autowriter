using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.RazorPages.Tests.IntegrationTests.Pages.Detail
{
    public class WhenIVisitTheUploadDetailPage : GivenAnHttpClient
    {
        private const string Url = "/upload/details?id=1";
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
            GivenTheHttpClientIsAuthenticated();
            await WhenISubmitTheForm("/upload", _testFormContent);

            await WhenIVisitThePageAt("/upload/details?id=1");

            TheResponseStatusIs(HttpStatusCode.OK);
        }

        [Fact]
        public async Task BodyContainsSourceMaterialText()
        {
            GivenTheHttpClientIsAuthenticated();
            await WhenISubmitTheForm("/upload", _testFormContent);

            await WhenIVisitThePageAt("/upload/details?id=1");

            await TheResponseBodyContains($"Uploaded on {DateTime.Now:dddd, d MMMM yyyy} at {DateTime.Now:H:mm tt}");
        }
    }
}
