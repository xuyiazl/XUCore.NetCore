using Microsoft.Extensions.Configuration;
using XUCore.Configs;
using XUCore.Tests.Samples;
using Xunit;

namespace XUCore.Tests.Configs
{
    public class ConfigUtilTest
    {
        [Fact]
        public void Test_GetJsonConfig()
        {
            var config = ConfigHelper.GetJsonConfig("sampleConfig.json", "Configs")
                .GetSection("Sample")
                .Get<SampleConfig>();
            Assert.Equal("TestSample", config.StringValue);
            Assert.Equal(20, config.DecimalValue);
            Assert.Equal(1, config.IntValue);
        }
    }
}