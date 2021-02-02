using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Sign
{
    public class SignOptions
    {
        /// <summary>
        /// header 前缀
        /// </summary>
        public string Prefix { get; set; } = "x-client-";
        /// <summary>
        /// 超时时间（秒）
        /// </summary>
        public int TimeOut { get; set; } = 60;
        /// <summary>
        /// 是否开启验证
        /// </summary>
        public bool IsOpen { get; set; } = true;
    }
}
