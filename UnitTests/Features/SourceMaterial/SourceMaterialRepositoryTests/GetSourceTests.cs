using System;
using System.Data;
using AutoMapper;
using Autowriter.Features.SourceMaterial;
using Dapper;
using Microsoft.Data.Sqlite;
using Xunit;

namespace UnitTests.Features.SourceMaterial.SourceMaterialRepositoryTests
{
    public class GetSourceTests
    {
        private readonly IDbConnection _conn;
        private readonly SourceMaterialRepository _repo;

        public GetSourceTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new Autowriter.Features.SourceMaterial.AutoMapper()));
            _conn = new SqliteConnection("Data Source=:memory:");
            _repo = new SourceMaterialRepository(_conn);
        }

        [Fact]
        public void WhenASourceDoesNotExist_GetSourceReturnsNull()
        {
            var source = _repo.GetSource(1);

            Assert.Null(source);
        }

        [Fact]
        public void WhenSourceWithIdOfOneExists_GetSourceReturnsSource()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            CreateSource(createdDate, content);

            var source = _repo.GetSource(1);

            Assert.Equal(1, source?.Id);
            Assert.Equal(createdDate, source?.Created);
            Assert.Equal(content, source?.Content);
        }

        [Fact]
        public void WhenSourceWithIdOfTwoExists_GetSourceReturnsSource()
        {
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content 1";
            var secondCreatedDate = new DateTime(2021, 12, 4, 16, 17, 0);
            var secondContent = "Unit test content 2";

            CreateSource(firstCreatedDate, firstContent);
            CreateSource(secondCreatedDate, secondContent);
            var source = _repo.GetSource(2);

            Assert.Equal(2, source?.Id);
            Assert.Equal(secondCreatedDate, source?.Created);
            Assert.Equal(secondContent, source?.Content);
        }

        private void CreateSource(DateTime created, string content)
        {
            const string query = $"INSERT INTO {SourceMaterialRepository.TableName} (created, content) VALUES (@created, @content)";
            var parameters = new { created, content };
            _conn.Execute(query, parameters);
        }
    }
}
