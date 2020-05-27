
using System;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.Tests;
using Xunit;
using Xunit.Abstractions;

namespace XUCore.Tests.Helpers
{
    /// <summary>
    /// Unix时间操作测试
    /// </summary>
    public class UnixTimeTest : TestBase
    {
        public UnixTimeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact(Skip = "由于运行时间，可能存在延迟")]
        public void Test_ToTimestamp()
        {
            var dto = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            var result = UnixTime.ToTimestamp(false);
            Output.WriteLine(dto.ToString());
            Output.WriteLine(result.ToString());
            Assert.Equal(dto, result);
        }

        [Fact]
        public void Test_ToTimestamp_2()
        {
            var result = Time.GetTimeFromUnixTimestamp(1582520001);
            Output.WriteLine(result.ToDateTimeString());
        }
    }
}
