using XUCore.Randoms;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.Tests
{
    /// <summary>
    /// 伪随机数生成器。用于单元测试，固定返回字符串"random"
    /// </summary>
    public class StubRandomGenerator : IRandomGenerator
    {
        /// <summary>
        /// 生成随机数
        /// </summary>
        public string Generate() => "random";
    }
}
