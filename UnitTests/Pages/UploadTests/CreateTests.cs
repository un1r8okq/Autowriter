using Autowriter.Database;
using Autowriter.Pages.Upload;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Moq.It;

namespace UnitTests.Pages.UploadTests
{
    public class CreateTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task WhenTextIsNullOrEmpty_EmptyTextFlagIsTrue(string text)
        {
            var db = Mock.Of<IDataRepository>();
            IRequestHandler<CreateHandler.Command, CreateHandler.Model> sut = new CreateHandler.Handler(db);
            var command = new CreateHandler.Command { Text = text };

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.True(result.TextWasEmpty);
        }

        [Fact]
        public async Task WhenTextHasContent_EmptyTextFlagIsFalse()
        {
            var db = Mock.Of<IDataRepository>();
            IRequestHandler<CreateHandler.Command, CreateHandler.Model> sut = new CreateHandler.Handler(db);
            var command = new CreateHandler.Command { Text = "Some text" };

            var result = await sut.Handle(command, CancellationToken.None);

            Assert.False(result.TextWasEmpty);
        }

        [Fact]
        public async Task WhenTextHasContent_QueryIsExecuted()
        {
            var db = new Mock<IDataRepository>();
            db
                .Setup(x => x.Execute(IsAny<string>(), IsAny<object>()))
                .Returns(1);
            IRequestHandler<CreateHandler.Command, CreateHandler.Model> sut = new CreateHandler.Handler(db.Object);
            var command = new CreateHandler.Command { Text = "Some text" };

            var result = await sut.Handle(command, CancellationToken.None);

            db.Verify(x => x.Execute(IsAny<string>(), IsAny<object>()), Times.Once);
        }
    }
}
