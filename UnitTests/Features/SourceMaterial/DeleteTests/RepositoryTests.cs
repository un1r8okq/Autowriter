using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autowriter.Core.Features.SourceMaterial;
using Dapper;
using Xunit;

namespace UnitTests.Features.SourceMaterial.DeleteTests
{
    public class RepositoryTests : CoreDbBackedTest
    {
        private const string TableName = "source_material";
        private readonly Delete.Repository _repo;

        public RepositoryTests()
        {
            _repo = new Delete.Repository(_conn);
        }

        [Fact]
        public void WhenASourceDoesNotExist_DeleteSourceDoesNothing()
        {
            _repo.DeleteSource(1);
        }

        [Fact]
        public void WhenSourceExists_AndSourceIsDeleted_SourceNoLongerExists()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            CreateSource(createdDate, content);

            _repo.DeleteSource(1);

            Assert.Null(GetSource(1));
            Assert.Empty(GetSources());
        }

        [Fact]
        public void WhenTwoSourcesExist_AndSourceOneIsDeleted_SourceTwoRemains()
        {
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content 1";
            var secondCreatedDate = new DateTime(2021, 12, 4, 16, 17, 0);
            var secondContent = "Unit test content 2";
            CreateSource(firstCreatedDate, firstContent);
            CreateSource(secondCreatedDate, secondContent);

            _repo.DeleteSource(1);
            var source = GetSource(2);

            Assert.Equal(2, source?.Id);
            Assert.Equal(secondCreatedDate, source?.Created);
            Assert.Equal(secondContent, source?.Content);
        }

        [Fact]
        public void WhenTwoSourcesExist_AndSourceTwoIsDeleted_SourceOneRemains()
        {
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content 1";
            var secondCreatedDate = new DateTime(2021, 12, 4, 16, 17, 0);
            var secondContent = "Unit test content 2";
            CreateSource(firstCreatedDate, firstContent);
            CreateSource(secondCreatedDate, secondContent);

            _repo.DeleteSource(2);
            var source = GetSource(1);

            Assert.Equal(1, source?.Id);
            Assert.Equal(firstCreatedDate, source?.Created);
            Assert.Equal(firstContent, source?.Content);
        }

        private void CreateSource(DateTime created, string content)
        {
            const string query = $"INSERT INTO {TableName} (created, content) VALUES (@created, @content)";
            var parameters = new { created, content };
            _conn.Execute(query, parameters);
        }

        private SourceMaterial? GetSource(int id) =>
            _conn
                .Query<SourceMaterial>($"SELECT id, created, content FROM {TableName} WHERE id = @id", new { id })
                .OrderByDescending(model => model.Created)
                .FirstOrDefault();

        private IEnumerable<SourceMaterial> GetSources() =>
            _conn
                .Query<SourceMaterial>($"SELECT id, created, content FROM {TableName}")
                .OrderByDescending(model => model.Created);

        private class SourceMaterial
        {
            public int Id { get; set; }

            public DateTime Created { get; set; }

            public string Content { get; set; } = string.Empty;
        }
    }
}
