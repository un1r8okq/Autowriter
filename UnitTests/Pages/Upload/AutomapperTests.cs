using Xunit;

namespace UnitTests.Pages.Upload
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Autowriter.RazorPages.Pages.Upload.AutoMapperConfig()));

            config.AssertConfigurationIsValid();
        }
    }
}
