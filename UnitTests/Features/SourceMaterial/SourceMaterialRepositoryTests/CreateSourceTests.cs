using System;
using System.Linq;
using AutoMapper;
using Autowriter;
using Autowriter.Features.SourceMaterial;
using Microsoft.Data.Sqlite;
using Xunit;

namespace UnitTests.Features.SourceMaterial.SourceMaterialRepositoryTests
{
    public class CreateSourceTests
    {
        private readonly SourceMaterialRepository _repo;

        public CreateSourceTests()
        {
            var mapperConfig = new MapperConfiguration(m => m.AddProfile<AutoMapperProfile>());
            var dbConnection = new SqliteConnection("Data Source=:memory:");

            _repo = new SourceMaterialRepository(dbConnection);
        }

        [Fact]
        public void WhenNoSourcesExist_AndSourceIsCreated_GetSourcesReturnsOneSource()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            _repo.CreateSource(createdDate, content);
            var sources = _repo.GetSources();

            Assert.Single(sources);
        }

        [Fact]
        public void WhenNoSourcesExist_AndIdenticalSourcesAreCreated_GetSourcesReturnsTwoSources()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            _repo.CreateSource(createdDate, content);
            _repo.CreateSource(createdDate, content);
            var sources = _repo.GetSources();

            Assert.Equal(2, sources.Count());
        }

        [Fact]
        public void WhenNoSourcesExist_AndSourceIsCreated_GetSourcesContainsSource_WithIdOfOne()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            _repo.CreateSource(createdDate, content);
            var sources = _repo.GetSources();

            Assert.Contains(sources, source => source.Id == 1);
        }

        [Fact]
        public void WhenOneSourceExists_AndSourceIsCreated_GetSourcesContainsSource_WithIdOfTwo()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            _repo.CreateSource(createdDate, content);
            _repo.CreateSource(createdDate, content);
            var sources = _repo.GetSources();

            Assert.Contains(sources, source => source.Id == 2);
        }

        [Fact]
        public void WhenOneSourceExists_AndUniqueSourceIsCreated_GetSourcesContainsFirstSource()
        {
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content 1";
            var secondCreatedDate = new DateTime(2021, 12, 4, 16, 17, 0);
            var secondContent = "Unit test content 2";

            _repo.CreateSource(firstCreatedDate, firstContent);
            _repo.CreateSource(secondCreatedDate, secondContent);
            var sources = _repo.GetSources();

            Assert.Contains(sources, (source) => source.Created == firstCreatedDate);
            Assert.Contains(sources, (source) => source.Content == firstContent);
        }

        [Fact]
        public void WhenOneSourceExists_AndUniqueSourceIsCreated_GetSourcesContainsSecondSource()
        {
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content";
            var secondCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var secondContent = "Unit test content";

            _repo.CreateSource(firstCreatedDate, firstContent);
            _repo.CreateSource(secondCreatedDate, secondContent);
            var sources = _repo.GetSources();

            Assert.Contains(sources, (source) => source.Created == secondCreatedDate);
            Assert.Contains(sources, (source) => source.Content == secondContent);
        }
    }
}
