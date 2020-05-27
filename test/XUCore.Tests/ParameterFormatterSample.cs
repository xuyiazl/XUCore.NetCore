using XUCore.Parameters.Formats;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.Tests
{
    /// <summary>
    /// 参数格式化器样例
    /// </summary>
    public class ParameterFormatterSample : ParameterFormatBase
    {
        /// <summary>
        /// 格式化分割符
        /// </summary>
        protected override string FormatSeparator => ":";

        /// <summary>
        /// 连接符
        /// </summary>
        protected override string JoinSeparator => "|";
    }
}
