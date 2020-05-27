using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Redis
{
    /// <summary>
    /// 连接类型
    /// </summary>
    public enum ConnectTypeEnum
    {
        /// <summary>
        /// 只读连接
        /// </summary>
        Read,
        /// <summary>
        /// 只写连接
        /// </summary>
        Write,
        /// <summary>
        /// 读写操作连接
        /// </summary>
        ReadAndWrite,
    }


    /// <summary>
    /// 值覆盖类型枚举
    /// </summary>
    public enum OverWrittenTypeDenum : short
    {
        /// <summary>
        /// 总是覆盖
        /// </summary>
        Always = 0,
        /// <summary>
        /// 仅存在再覆盖
        /// </summary>
        Exists = 1,
        /// <summary>
        /// 不存在的时候再覆盖
        /// </summary>
        NotExists = 2
    }
}
