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

namespace UnitTests.Pages.UploadTests
{
    public class CreateTests
    {
        private readonly IMapper _mapper =
            new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile())).CreateMapper();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task WhenTextIsNullOrEmpty_EmptyTextFlagIsTrue(string text)
        {
            var repository = Mock.Of<ISourceMaterialRepository>();
            IRequestHandler<CreateHandler.Command, CreateHandler.Response> sut = new CreateHandler.Handler(repository, _mapper);
            var command = new CreateHandler.Command { Content = text };

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.True(result.TextWasEmpty);
        }

        [Fact]
        public async Task WhenTextHasContent_EmptyTextFlagIsFalse()
        {
            var repository = Mock.Of<ISourceMaterialRepository>();
            IRequestHandler<CreateHandler.Command, CreateHandler.Response> sut = new CreateHandler.Handler(repository, _mapper);
            var command = new CreateHandler.Command { Content = "Some text" };

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.False(result.TextWasEmpty);
        }

        [Fact]
        public async Task WhenTextHasContent_QueryIsExecuted()
        {
            var repository = new Mock<ISourceMaterialRepository>();
            IRequestHandler<CreateHandler.Command, CreateHandler.Response> sut = new CreateHandler.Handler(repository.Object, _mapper);
            var command = new CreateHandler.Command { Content = "Some text" };

            var result = await sut.Handle(command, CancellationToken.None);

            repository.Verify(x => x.CreateSource(It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);
        }
    }
}
