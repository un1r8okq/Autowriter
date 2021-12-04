using Autowriter.Data;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.Data.SourceMaterialRepositoryTests
{
    public class GetSourcesTests
    {
        [Fact]
        public void WhenNoSourcesExist_GetSourcesReturnsEmptyCollection()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);

            var sources = repo.GetSources();

            Assert.Empty(sources);
        }

        [Fact]
        public void WhenTenSourcesExist_GetSourcesReturnsTenSources()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            for (var i = 0; i < 10; i++)
            {
                repo.CreateSource(createdDate, content);
            }

            var sources = repo.GetSources();

            Assert.Equal(10, sources.Count());
        }

        [Fact]
        public void WhenOneSourceExists_AndSourceCreatedDatetimeIsEdited_GetSourcesReturnsUneditedSource()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            repo.CreateSource(createdDate, content);

            var source = repo.GetSources().First();
            source.Created = DateTime.MinValue;

            Assert.Equal(createdDate, repo.GetSources().First().Created);
        }

        [Fact]
        public void WhenOneSourceExists_AndSourceContentIsEdited_GetSourcesReturnsUneditedSource()
        {
            var conn = new SqliteConnection("Data Source=:memory:");
            var repo = new SourceMaterialRepository(conn);
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            repo.CreateSource(createdDate, content);

            var source = repo.GetSources().First();
            source.Content = string.Empty;

            Assert.Equal(content, repo.GetSources().First().Content);
        }
    }
}
