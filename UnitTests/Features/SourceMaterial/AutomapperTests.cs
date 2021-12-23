using Xunit;

namespace UnitTests.Features.SourceMaterial
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Autowriter.Features.SourceMaterial.AutoMapper()));

            config.AssertConfigurationIsValid();
        }
    }
}
