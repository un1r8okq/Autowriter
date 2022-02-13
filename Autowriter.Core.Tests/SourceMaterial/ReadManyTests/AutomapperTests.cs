using Xunit;

namespace Autowriter.Core.Tests.SourceMaterial.ReadManyTests
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Features.SourceMaterial.ReadMany.AutoMapper()));

            config.AssertConfigurationIsValid();
        }
    }
}
