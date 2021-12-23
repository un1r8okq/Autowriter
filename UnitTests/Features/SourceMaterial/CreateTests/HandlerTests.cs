using System;
using System.Threading;
using System.Threading.Tasks;
using Autowriter.Features.SourceMaterial;
using MediatR;
using Moq;
using Xunit;

namespace UnitTests.Features.SourceMaterial.CreateTests
{
    public class HandlerTests
    {
        private readonly Mock<Create.ICreateSourceMaterial> _createSourceMock;
        private readonly IRequestHandler<Create.Command, Create.Response> _handler;

        public HandlerTests()
        {
            _createSourceMock = new Mock<Create.ICreateSourceMaterial>();
            _handler = new Create.Handler(_createSourceMock.Object);
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

        [Fact]
        public async Task WhenTextHasContent_QueryIsExecuted()
        {
            var command = new Create.Command { Content = "Some text" };

            var result = await _handler.Handle(command, CancellationToken.None);

            _createSourceMock.Verify(x => x.CreateSource(It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);
        }
    }
}
