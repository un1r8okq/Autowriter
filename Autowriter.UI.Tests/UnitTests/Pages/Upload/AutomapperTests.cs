using Xunit;

namespace Autowriter.UI.Tests.UnitTests.Pages.Upload
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new UI.Pages.Upload.AutoMapperConfig()));

            config.AssertConfigurationIsValid();
        }
    }
}
