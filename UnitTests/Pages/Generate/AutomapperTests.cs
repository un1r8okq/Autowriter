using Xunit;

namespace UnitTests.Pages.Generate
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Autowriter.RazorPages.Pages.Generate.AutoMapperConfig()));

            config.AssertConfigurationIsValid();
        }
    }
}
