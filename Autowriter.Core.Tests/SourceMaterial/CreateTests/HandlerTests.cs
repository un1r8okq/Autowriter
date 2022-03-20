using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Autowriter.Core.Features.SourceMaterial;
using MediatR;
using Xunit;

namespace Autowriter.Core.Tests.SourceMaterial.CreateTests
{
    public class HandlerTests
    {
        private readonly IRequestHandler<Create.Command, Create.Response> _handler;

        public HandlerTests()
        {
            var createSourceStub = new CreateSourceStub();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new Create.AutoMapper()));
            var mapper = mapperConfig.CreateMapper();
            _handler = new Create.Handler(createSourceStub, mapper);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task WhenTextIsNullOrEmpty_EmptyTextFlagIsTrue(string text)
        {
            var command = new Create.Command { Content = text };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.TextWasEmpty);
        }

        [Fact]
        public async Task WhenTextHasContent_EmptyTextFlagIsFalse()
        {
            var command = new Create.Command { Content = "Some text" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.TextWasEmpty);
        }

        private class CreateSourceStub : Create.ICreateSourceMaterial
        {
            public Create.Repository.SourceMaterial CreateSource(DateTime createdDateTime, string content) => new ();
        }
    }
}
