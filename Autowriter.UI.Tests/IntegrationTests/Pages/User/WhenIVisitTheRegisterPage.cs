using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Autowriter.UI.Tests.IntegrationTests.Pages.User
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

        [Fact]
        public async Task AndTheFormIsValid_AndISubmitTheForm_IAmRedirectedToLogin()
        {
            var email = $"{Guid.NewGuid()}@example.com";
            var formBody = new Dictionary<string, string>
            {
                { "email", email },
                { "password", "password" },
            };
            await WhenISubmitTheForm("/user/register", formBody);

            TheResponseStatusIs(HttpStatusCode.Redirect);
            TheLocationHeaderIs($"/user/login?email={email}");
        }

        [Fact]
        public async Task AndTheEmailIsInvalid_AndISubmitTheForm_TheResponseContainsValidationMessages()
        {
            var formBody = new Dictionary<string, string>
            {
                { "email", "NotAnEmailAddress" },
                { "password", string.Empty },
            };
            await WhenISubmitTheForm("/user/register", formBody);

            TheResponseStatusIs(HttpStatusCode.OK);
            await TheResponseBodyContains("The Email field is not a valid e-mail address.");
            await TheResponseBodyContains("The Password field is required.");
        }

        [Fact]
        public async Task AndTheUsernameAlreadyExists_TheResponseContainsErrorMessage()
        {
            var formBody = new Dictionary<string, string>
            {
                { "email", "test@example.com" },
                { "password", "password" },
            };
            await WhenISubmitTheForm("/user/register", formBody);
            await WhenISubmitTheForm("/user/register", formBody);

            TheResponseStatusIs(HttpStatusCode.OK);
            await TheResponseBodyContains("Error creating user:");
            await TheResponseBodyContains("Username &#x27;test@example.com&#x27; is already taken.");
        }
    }
}
