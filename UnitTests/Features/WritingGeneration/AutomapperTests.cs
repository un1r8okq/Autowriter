using Xunit;

namespace UnitTests.Features.WritingGeneration
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new AutoMapper.MapperConfiguration(mapper =>
                mapper.AddProfile(new Autowriter.Features.WritingGeneration.AutoMapper()));

            config.AssertConfigurationIsValid();
        }
    }
}
