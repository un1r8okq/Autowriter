using Xunit;

namespace Autowriter.RazorPages.Tests.UnitTests.Pages.Generate
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new RazorPages.Pages.Generate.AutoMapperConfig()));

            config.AssertConfigurationIsValid();
        }
    }
}
