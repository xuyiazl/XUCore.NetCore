using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XUCore.NetCore.HttpFactory
{
    /// <summary>
    /// 数据MediaType类型
    /// </summary>
    public enum HttpMediaType
    {
        /// <summary>
        /// 返回的数据类型为JSON
        /// </summary>
        [Description("application/json")]
        Json = 1,
        /// <summary>
        /// 返回的数据类型为MessagePack
        /// </summary>
        [Description("application/x-msgpack")]
        MessagePack = 2,
        /// <summary>
        /// 返回的数据类型为MessagePack-Jackson
        /// </summary>
        [Description("application/x-msgpack-jackson")]
        MessagePackJackson = 3
    }
}
