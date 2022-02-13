using Xunit;

namespace Autowriter.Core.Tests.SourceMaterial.ReadSingleTests
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Features.SourceMaterial.ReadSingle.AutoMapper()));

            config.AssertConfigurationIsValid();
        }
    }
}
