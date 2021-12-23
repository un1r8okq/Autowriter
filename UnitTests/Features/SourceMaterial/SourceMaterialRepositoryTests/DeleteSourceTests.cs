using AutoMapper;
using Autowriter;
using Autowriter.Features.SourceMaterial;
using Microsoft.Data.Sqlite;
using System;
using Xunit;

namespace UnitTests.Features.SourceMaterial.SourceMaterialRepositoryTests
{
    public class DeleteSourceTests
    {
        private readonly SourceMaterialRepository _repo;

        public DeleteSourceTests()
        {
            var mapperConfig = new MapperConfiguration(m => m.AddProfile<AutoMapperProfile>());
            var dbConnection = new SqliteConnection("Data Source=:memory:");

            _repo = new SourceMaterialRepository(dbConnection);
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
            _repo.CreateSource(createdDate, content);

            _repo.DeleteSource(1);

            Assert.Null(_repo.GetSource(1));
            Assert.Empty(_repo.GetSources());
        }

        [Fact]
        public void WhenTwoSourcesExist_AndSourceOneIsDeleted_SourceTwoRemains()
        {
            var firstCreatedDate = new DateTime(2021, 12, 4, 15, 47, 0);
            var firstContent = "Unit test content 1";
            var secondCreatedDate = new DateTime(2021, 12, 4, 16, 17, 0);
            var secondContent = "Unit test content 2";
            _repo.CreateSource(firstCreatedDate, firstContent);
            _repo.CreateSource(secondCreatedDate, secondContent);

            _repo.DeleteSource(1);
            var source = _repo.GetSource(2);

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
            _repo.CreateSource(firstCreatedDate, firstContent);
            _repo.CreateSource(secondCreatedDate, secondContent);

            _repo.DeleteSource(2);
            var source = _repo.GetSource(1);

            Assert.Equal(1, source?.Id);
            Assert.Equal(firstCreatedDate, source?.Created);
            Assert.Equal(firstContent, source?.Content);
        }
    }
}
