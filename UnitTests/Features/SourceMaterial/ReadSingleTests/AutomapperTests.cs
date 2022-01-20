using Xunit;

namespace UnitTests.Features.SourceMaterial.ReadSingleTests
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Autowriter.Core.Features.SourceMaterial.ReadSingle.AutoMapper()));

            config.AssertConfigurationIsValid();
        }
    }
}
