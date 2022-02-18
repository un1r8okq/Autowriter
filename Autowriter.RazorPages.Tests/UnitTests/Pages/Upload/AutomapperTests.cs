using Xunit;

namespace Autowriter.RazorPages.Tests.UnitTests.Pages.Upload
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new RazorPages.Pages.Upload.AutoMapperConfig()));

            config.AssertConfigurationIsValid();
        }
    }
}
