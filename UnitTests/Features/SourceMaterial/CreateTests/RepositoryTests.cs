using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AutoMapper;
using Autowriter.Features.SourceMaterial;
using Dapper;
using Xunit;

namespace UnitTests.Features.SourceMaterial.CreateTests
{
    public class RepositoryTests : SqliteBackedTest
    {
        private readonly Create.Repository _repo;

        public RepositoryTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new Autowriter.Features.SourceMaterial.AutoMapper()));
            _repo = new Create.Repository(_conn);
        }

        [Fact]
        public void WhenNoSourcesExist_AndSourceIsCreated_GetSourcesReturnsOneSource()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            _repo.CreateSource(createdDate, content);
            var sources = GetSources();

            Assert.Single(sources);
        }

        [Fact]
        public void WhenNoSourcesExist_AndIdenticalSourcesAreCreated_GetSourcesReturnsTwoSources()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            _repo.CreateSource(createdDate, content);
            _repo.CreateSource(createdDate, content);
            var sources = GetSources();

            Assert.Equal(2, sources.Count());
        }

        [Fact]
        public void WhenNoSourcesExist_AndSourceIsCreated_GetSourcesContainsSource_WithIdOfOne()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            _repo.CreateSource(createdDate, content);
            var sources = GetSources();

            Assert.Contains(sources, source => source.Id == 1);
        }

        [Fact]
        public void WhenOneSourceExists_AndSourceIsCreated_GetSourcesContainsSource_WithIdOfTwo()
        {
            var createdDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var content = "Unit test content";

            _repo.CreateSource(createdDate, content);
            _repo.CreateSource(createdDate, content);
            var sources = GetSources();

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
            var sources = GetSources();

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
            var sources = GetSources();

            Assert.Contains(sources, (source) => source.Created == secondCreatedDate);
            Assert.Contains(sources, (source) => source.Content == secondContent);
        }

        private IEnumerable<Create.Response.SourceMaterial> GetSources() =>
            _conn
                .Query<Create.Response.SourceMaterial>("SELECT id, created, content FROM source_material")
                .OrderByDescending(model => model.Created);
    }
}
