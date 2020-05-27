using System.IO;
using XUCore.Tests;
using XUCore.IO;
using Xunit;
using Xunit.Abstractions;

namespace XUCore.Tests.IO
{
    /// <summary>
    /// 目录操作辅助类测试
    /// </summary>
    public class DirectoryUtilTest:TestBase
    {
        public DirectoryUtilTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Test_GetFileNames()
        {
            var result = DirectoryHelper.GetFileNames(Directory.GetCurrentDirectory());
            foreach (var item in result)
            {
                Output.WriteLine(item);
            }
        }
    }
}
