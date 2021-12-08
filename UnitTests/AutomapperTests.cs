using AutoMapper;
using Autowriter;
using Xunit;

namespace UnitTests
{
    public class AutomapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            var config = new MapperConfiguration(mapper =>
                mapper.AddProfile(new AutoMapperProfile()));

            config.AssertConfigurationIsValid();
        }
    }
}
