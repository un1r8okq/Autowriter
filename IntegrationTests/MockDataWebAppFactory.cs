using Autowriter.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;

namespace IntegrationTests
{
    public class MockDataWebAppFactory<TProgram>: WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var testSources = new SourceMaterialRepository.Source[]
            {
                new SourceMaterialRepository.Source
                {
                    Id = 1,
                    Created = new DateTime(2021, 11, 28, 18, 28, 0),
                    Content = "Integration test data one",
                },
                new SourceMaterialRepository.Source
                {
                    Id = 2,
                    Created = new DateTime(2021, 11, 28, 18, 29, 0),
                    Content = "Integration test data two",
                },
            };

            var mockRepo = new Mock<ISourceMaterialRepository>();
            mockRepo
                .Setup(x => x.GetSource(It.IsAny<int>()))
                .Returns(testSources.First());
            mockRepo
                .Setup(x => x.GetSources())
                .Returns(testSources);

            builder.ConfigureTestServices(config => config.AddSingleton(mockRepo.Object));
        }
    }
}
