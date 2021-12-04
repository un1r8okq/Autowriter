using Autowriter.Data;
using Microsoft.Data.Sqlite;
using System;
using Xunit;

namespace UnitTests.Data.SourceMaterialRepositoryTests
{
    public class DeleteSourceTests
    {
        [Fact]
        public void WhenASourceDoesNotExist_DeleteSourceDoesNothing()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);

            repo.DeleteSource(1);
        }

        [Fact]
        public void WhenSourceExists_AndSourceIsDeleted_SourceNoLongerExists()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            repo.CreateSource(createdDate, content);

            repo.DeleteSource(1);

            Assert.Null(repo.GetSource(1));
            Assert.Empty(repo.GetSources());
        }

        [Fact]
        public void WhenTwoSourcesExist_AndSourceOneIsDeleted_SourceTwoRemains()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content 1";
            var secondCreatedDate = new DateTime(2021, 12, 4, 16, 17, 0);
            var secondContent = "Unit test content 2";
            repo.CreateSource(firstCreatedDate, firstContent);
            repo.CreateSource(secondCreatedDate, secondContent);

            repo.DeleteSource(1);
            var source = repo.GetSource(2);

            Assert.Equal(2, source?.Id);
            Assert.Equal(secondCreatedDate, source?.Created);
            Assert.Equal(secondContent, source?.Content);
        }

        [Fact]
        public void WhenTwoSourcesExist_AndSourceTwoIsDeleted_SourceOneRemains()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content 1";
            var secondCreatedDate = new DateTime(2021, 12, 4, 16, 17, 0);
            var secondContent = "Unit test content 2";
            repo.CreateSource(firstCreatedDate, firstContent);
            repo.CreateSource(secondCreatedDate, secondContent);

            repo.DeleteSource(2);
            var source = repo.GetSource(1);

            Assert.Equal(1, source?.Id);
            Assert.Equal(firstCreatedDate, source?.Created);
            Assert.Equal(firstContent, source?.Content);
        }
    }
}
