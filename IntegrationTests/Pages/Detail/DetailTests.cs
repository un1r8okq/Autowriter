using Autowriter.Database;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Pages.Generate
{
    public class DetailTests
    {
        private const string Url = "/upload/details?id=1";
        private readonly HttpClient _client;

        public DetailTests()
        {
            var dataRepository = CreateTestDataRepository();
            var app = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(webHostBuilder =>
                    webHostBuilder.ConfigureTestServices(services =>
                        services.AddSingleton(dataRepository)));
            _client = app.CreateClient();
        }

        [Fact]
        public async Task ReturnsOK()
        {
            var response = await _client.GetAsync(Url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task BodyContainsSourceMaterialText()
        {
            var response = await _client.GetAsync(Url);
            var body = await response.Content.ReadAsStringAsync();

            Assert.Contains("Integration test", body);
        }

        private static IDataRepository CreateTestDataRepository()
        {
            var dbConnection = new SqliteConnection("Data Source=:memory:");
            dbConnection.Open();
            new DbBootstrapper(dbConnection).Bootstrap();
            var dataRepository = new DataRepository(dbConnection);
            var query = $"INSERT INTO {TableNames.SourceMaterial}" +
                            "(id, created, text) VALUES (@id, @created, @text)";
            var parameters = new
            {
                id = 1,
                created = new DateTime(2021, 11, 28, 11, 37, 00),
                text = "Integration test",
            };
            dataRepository.Execute(query, parameters);

            return dataRepository;
        }
    }
}
