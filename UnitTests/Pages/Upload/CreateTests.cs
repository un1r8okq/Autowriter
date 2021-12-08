using AutoMapper;
using Autowriter;
using Autowriter.Data;
using Autowriter.Pages.Upload;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Pages.Upload
{
    public class CreateTests
    {
        private readonly Mock<ICreateSourceMaterial> _createSourceMock;
        private readonly IRequestHandler<CreateHandler.Command, CreateHandler.Response> _handler;

        public CreateTests()
        {
            _createSourceMock = new Mock<ICreateSourceMaterial>();
            var readSourceMock = new Mock<IReadSourceMaterials>();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            var mapper = mapperConfig.CreateMapper();

            _handler = new CreateHandler.Handler(_createSourceMock.Object, readSourceMock.Object, mapper);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task WhenTextIsNullOrEmpty_EmptyTextFlagIsTrue(string text)
        {
            var command = new CreateHandler.Command { Content = text };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.TextWasEmpty);
        }

        [Fact]
        public async Task WhenTextHasContent_EmptyTextFlagIsFalse()
        {
            var command = new CreateHandler.Command { Content = "Some text" };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.TextWasEmpty);
        }

        [Fact]
        public async Task WhenTextHasContent_QueryIsExecuted()
        {
            var command = new CreateHandler.Command { Content = "Some text" };

            var result = await _handler.Handle(command, CancellationToken.None);

            _createSourceMock.Verify(x => x.CreateSource(It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);
        }
    }
}
