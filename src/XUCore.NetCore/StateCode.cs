using System.ComponentModel;

namespace XUCore.NetCore
{
    /// <summary>
    /// 状态码
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Ok = 0,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = -1
    }
}