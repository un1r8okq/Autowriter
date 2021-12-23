using System;
using System.Data;
using System.Linq;
using AutoMapper;
using Autowriter.Features.SourceMaterial;
using Microsoft.Data.Sqlite;
using Xunit;

namespace UnitTests.Features.SourceMaterial.SourceMaterialRepositoryTests
{
    public class GetSourcesTests
    {
        private readonly IDbConnection _conn;
        private readonly SourceMaterialRepository _repo;

        public GetSourcesTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new Autowriter.Features.SourceMaterial.AutoMapper()));
            _conn = new SqliteConnection("Data Source=:memory:");
            _repo = new SourceMaterialRepository(_conn);
        }

        [Fact]
        public void WhenNoSourcesExist_GetSourcesReturnsEmptyCollection()
        {
            var sources = _repo.GetSources();

            Assert.Empty(sources);
        }

        [Fact]
        public void WhenTenSourcesExist_GetSourcesReturnsTenSources()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            for (var i = 0; i < 10; i++)
            {
                CreateSource(createdDate, content);
            }

            var sources = _repo.GetSources();

            Assert.Equal(10, sources.Count());
        }

        [Fact]
        public void WhenOneSourceExists_AndSourceCreatedDatetimeIsEdited_GetSourcesReturnsUneditedSource()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            CreateSource(createdDate, content);

            var source = _repo.GetSources().First();
            source.Created = DateTime.MinValue;

            Assert.Equal(createdDate, _repo.GetSources().First().Created);
        }

        [Fact]
        public void WhenOneSourceExists_AndSourceContentIsEdited_GetSourcesReturnsUneditedSource()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            CreateSource(createdDate, content);

            var source = _repo.GetSources().First();
            source.Content = string.Empty;

            Assert.Equal(content, _repo.GetSources().First().Content);
        }

        private void CreateSource(DateTime created, string content)
        {
            const string query = $"INSERT INTO {SourceMaterialRepository.TableName} (created, content) VALUES (@created, @content)";
            var parameters = new { created, content };
            _conn.Execute(query, parameters);
        }
    }
}
