using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autowriter.Core.Features.WritingGeneration;
using MediatR;
using Xunit;

namespace Autowriter.Core.Tests.WritingGeneration
{
    public class GenerateTests
    {
        private readonly IRequestHandler<Generate.Command, Generate.Response> _handler;

        public GenerateTests()
        {
            var readSourceStub = new ReadSourceStub();
            _handler = new Generate.Handler(readSourceStub);
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

        private class ReadSourceStub : Generate.IReadSourceMaterial
        {
            public IEnumerable<string> GetSources() => Array.Empty<string>();
        }
    }
}
