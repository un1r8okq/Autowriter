using Xunit;

namespace UnitTests.Features.SourceMaterial.ReadManyTests
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Autowriter.Features.SourceMaterial.ReadMany.AutoMapper()));

            config.AssertConfigurationIsValid();
        }
    }
}
