using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Autowriter.RazorPages.Tests.IntegrationTests
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostXsrfProtectedForm(
            this HttpClient client,
            string url,
            Dictionary<string, string> body)
        {
            var xsrfToken = await GetXsrfToken(client, url);
            body.Add("__RequestVerificationToken", xsrfToken);
            var content = new FormUrlEncodedContent(body);
            var response = await client.PostAsync(url, content);

            return response;
        }

        private static async Task<string> GetXsrfToken(HttpClient client, string url)
        {
            var pageResponse = await client.GetAsync(url);
            var pageBody = await pageResponse.Content.ReadAsStringAsync();
            var xsrfTokenRegex = new Regex("<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"(?<token>.+)\"");
            var xsrfToken = xsrfTokenRegex.Match(pageBody).Groups[1].Value;
            return xsrfToken;
        }
    }
}
