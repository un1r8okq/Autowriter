using Autowriter.Pages.Generate;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Pages.Generate
{
    public class GenerateTests
    {
        private readonly IRequestHandler<GenerateHandler.Command, GenerateHandler.Response> _handler;

        public GenerateTests()
        {
            var readSourceMock = Mock.Of<IReadSourceMaterial>();
            _handler = new GenerateHandler.Handler(readSourceMock);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(123)]
        [InlineData(1000)]
        [InlineData(1001)]
        public async Task RequestedNumberOfWords_IsAlwaysReturnedInTheResponse(int wordCount)
        {
            var command = new GenerateHandler.Command { WordCount = wordCount };

            var response = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(wordCount, response.RequestedNumberOfWords);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10001)]
        public async Task WhenRequestedNumberOfWordsIsNotWithinBounds_WordCountOutOfRange(int wordCount)
        {
            var command = new GenerateHandler.Command { WordCount = wordCount };

            var response = await _handler.Handle(command, CancellationToken.None);

            Assert.True(response.WordCountOutOfRange);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task WhenRequestedNumberOfWordsIsNotWithinBounds_WordCountNotOutOfRange(int wordCount)
        {
            var command = new GenerateHandler.Command { WordCount = wordCount };

            var response = await _handler.Handle(command, CancellationToken.None);

            Assert.False(response.WordCountOutOfRange);
        }
    }
}
