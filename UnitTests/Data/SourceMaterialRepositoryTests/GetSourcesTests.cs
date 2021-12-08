using AutoMapper;
using Autowriter;
using Autowriter.Data;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;
using Xunit;

namespace UnitTests.Data.SourceMaterialRepositoryTests
{
    public class GetSourcesTests
    {
        private readonly SourceMaterialRepository _repo;

        public GetSourcesTests()
        {
            var mapperConfig = new MapperConfiguration(m => m.AddProfile<AutoMapperProfile>());
            var mapper = mapperConfig.CreateMapper();
            var dbConnection = new SqliteConnection("Data Source=:memory:");

            _repo = new SourceMaterialRepository(dbConnection, mapper);
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
                _repo.CreateSource(createdDate, content);
            }

            var sources = _repo.GetSources();

            Assert.Equal(10, sources.Count());
        }

        [Fact]
        public void WhenOneSourceExists_AndSourceCreatedDatetimeIsEdited_GetSourcesReturnsUneditedSource()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            _repo.CreateSource(createdDate, content);

            var source = _repo.GetSources().First();
            source.Created = DateTime.MinValue;

            Assert.Equal(createdDate, _repo.GetSources().First().Created);
        }

        [Fact]
        public void WhenOneSourceExists_AndSourceContentIsEdited_GetSourcesReturnsUneditedSource()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";
            _repo.CreateSource(createdDate, content);

            var source = _repo.GetSources().First();
            source.Content = string.Empty;

            Assert.Equal(content, _repo.GetSources().First().Content);
        }
    }
}
