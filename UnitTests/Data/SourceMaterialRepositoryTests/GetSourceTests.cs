using AutoMapper;
using Autowriter;
using Autowriter.Data;
using Microsoft.Data.Sqlite;
using System;
using Xunit;

namespace UnitTests.Data.SourceMaterialRepositoryTests
{
    public class GetSourceTests
    {
        private readonly SourceMaterialRepository _repo;

        public GetSourceTests()
        {
            var mapperConfig = new MapperConfiguration(m => m.AddProfile<AutoMapperProfile>());
            var dbConnection = new SqliteConnection("Data Source=:memory:");

            _repo = new SourceMaterialRepository(dbConnection);
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
            _repo.CreateSource(createdDate, content);

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

            _repo.CreateSource(firstCreatedDate, firstContent);
            _repo.CreateSource(secondCreatedDate, secondContent);
            var source = _repo.GetSource(2);

            Assert.Equal(2, source?.Id);
            Assert.Equal(secondCreatedDate, source?.Created);
            Assert.Equal(secondContent, source?.Content);
        }
    }
}
