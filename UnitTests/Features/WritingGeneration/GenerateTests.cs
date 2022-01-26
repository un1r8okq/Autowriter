using System.Threading;
using System.Threading.Tasks;
using Autowriter.Core.Features.WritingGeneration;
using MediatR;
using Moq;
using Xunit;

namespace UnitTests.Features.WritingGeneration
{
    public class GenerateTests
    {
        private readonly IRequestHandler<Generate.Command, Generate.Response> _handler;

        public GenerateTests()
        {
            var readSourceMock = Mock.Of<Generate.IReadSourceMaterial>();
            _handler = new Generate.Handler(readSourceMock);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10001)]
        public async Task WhenRequestedNumberOfWordsIsNotWithinBounds_WordCountOutOfRange(int wordCount)
        {
            var command = new Generate.Command { WordCount = wordCount };

            var response = await _handler.Handle(command, CancellationToken.None);

            Assert.True(response.WordCountOutOfRange);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task WhenRequestedNumberOfWordsIsNotWithinBounds_WordCountNotOutOfRange(int wordCount)
        {
            var command = new Generate.Command { WordCount = wordCount };

            var response = await _handler.Handle(command, CancellationToken.None);

            Assert.False(response.WordCountOutOfRange);
        }
    }
}
