using Xunit;

namespace UnitTests.Features.SourceMaterial.CreateTests
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Autowriter.Core.Features.SourceMaterial.Create.AutoMapper()));

            config.AssertConfigurationIsValid();
        }
    }
}