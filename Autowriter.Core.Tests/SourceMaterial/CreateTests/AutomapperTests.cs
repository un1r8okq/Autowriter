using Xunit;

namespace Autowriter.Core.Tests.SourceMaterial.CreateTests
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Features.SourceMaterial.Create.AutoMapper()));

            config.AssertConfigurationIsValid();
        }
    }
}
