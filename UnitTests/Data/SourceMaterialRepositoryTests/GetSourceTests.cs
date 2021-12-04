using Autowriter.Data;
using Microsoft.Data.Sqlite;
using System;
using Xunit;

namespace UnitTests.Data.SourceMaterialRepositoryTests
{
    public class GetSourceTests
    {
        [Fact]
        public void WhenASourceDoesNotExist_GetSourceReturnsNull()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);

            var source = repo.GetSource(1);

            Assert.Null(source);
        }

        [Fact]
        public void WhenSourceWithIdOfOneExists_GetSourceReturnsSource()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            repo.CreateSource(createdDate, content);

            var source = repo.GetSource(1);

            Assert.Equal(1, source?.Id);
            Assert.Equal(createdDate, source?.Created);
            Assert.Equal(content, source?.Content);
        }

        [Fact]
        public void WhenSourceWithIdOfTwoExists_GetSourceReturnsSource()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content 1";
            var secondCreatedDate = new DateTime(2021, 12, 4, 16, 17, 0);
            var secondContent = "Unit test content 2";

            repo.CreateSource(firstCreatedDate, firstContent);
            repo.CreateSource(secondCreatedDate, secondContent);
            var source = repo.GetSource(2);

            Assert.Equal(2, source?.Id);
            Assert.Equal(secondCreatedDate, source?.Created);
            Assert.Equal(secondContent, source?.Content);
        }
    }
}
