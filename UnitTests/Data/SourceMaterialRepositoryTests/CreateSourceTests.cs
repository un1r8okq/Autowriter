using Autowriter.Data;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.Data.SourceMaterialRepositoryTests
{
    public class CreateSourceTests
    {
        [Fact]
        public void WhenNoSourcesExist_AndSourceIsCreated_GetSourcesReturnsOneSource()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            repo.CreateSource(createdDate, content);
            var sources = repo.GetSources();

            Assert.Single(sources);
        }

        [Fact]
        public void WhenNoSourcesExist_AndIdenticalSourcesAreCreated_GetSourcesReturnsTwoSources()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            repo.CreateSource(createdDate, content);
            repo.CreateSource(createdDate, content);
            var sources = repo.GetSources();

            Assert.Equal(2, sources.Count());
        }

        [Fact]
        public void WhenNoSourcesExist_AndSourceIsCreated_GetSourcesContainsSource_WithIdOfOne()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            repo.CreateSource(createdDate, content);
            var sources = repo.GetSources();

            Assert.Contains(sources, source => source.Id == 1);
        }

        [Fact]
        public void WhenOneSourceExists_AndSourceIsCreated_GetSourcesContainsSource_WithIdOfTwo()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            repo.CreateSource(createdDate, content);
            repo.CreateSource(createdDate, content);
            var sources = repo.GetSources();

            Assert.Contains(sources, source => source.Id == 2);
        }

        [Fact]
        public void WhenOneSourceExists_AndUniqueSourceIsCreated_GetSourcesContainsFirstSource()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content 1";
            var secondCreatedDate = new DateTime(2021, 12, 4, 16, 17, 0);
            var secondContent = "Unit test content 2";

            repo.CreateSource(firstCreatedDate, firstContent);
            repo.CreateSource(secondCreatedDate, secondContent);
            var sources = repo.GetSources();

            Assert.Contains(sources, (source) => source.Created == firstCreatedDate);
            Assert.Contains(sources, (source) => source.Content == firstContent);
        }

        [Fact]
        public void WhenOneSourceExists_AndUniqueSourceIsCreated_GetSourcesContainsSecondSource()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content";
            var secondCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var secondContent = "Unit test content";

            repo.CreateSource(firstCreatedDate, firstContent);
            repo.CreateSource(secondCreatedDate, secondContent);
            var sources = repo.GetSources();

            Assert.Contains(sources, (source) => source.Created == secondCreatedDate);
            Assert.Contains(sources, (source) => source.Content == secondContent);
        }
    }
}
