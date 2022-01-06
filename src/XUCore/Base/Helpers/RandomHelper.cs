using System;
using System.Collections.Generic;

namespace XUCore.Helpers
{
    /// <summary>
    /// 随机帮助类
    /// </summary>
    public static class RandomHelper
    {
        #region GetTel(随机生成电话号码)

        /// <summary>
        /// 手机号码段
        /// </summary>
        public static string[] TelStarts = "134,135,136,137,138,139,150,151,152,157,158,159,130,131,132,155,156,133,153,180,181,182,183,185,186,176,187,188,189,177,178".Split(',');

        /// <summary>
        /// 随机生成电话号码
        /// </summary>
        /// <returns></returns>
        public static string GetTel()
        {
            var ran = new Random();//随机数
            int index = ran.Next(0, TelStarts.Length - 1);
            string first = TelStarts[index];
            string second = (ran.Next(100, 888) + 10000).ToString().Substring(1);
            string thrid = (ran.Next(1, 9100) + 10000).ToString().Substring(1);
            return first + second + thrid;
        }

        #endregion GetType(获取类型)
    }
}