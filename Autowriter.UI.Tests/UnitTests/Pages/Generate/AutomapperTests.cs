using Xunit;

namespace Autowriter.UI.Tests.UnitTests.Pages.Generate
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new UI.Pages.Generate.AutoMapperConfig()));

            config.AssertConfigurationIsValid();
        }
    }
}
