using MessagePack;
using Microsoft.AspNetCore.Mvc;
using XUCore.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore
{
    /// <summary>
    /// 返回结构体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [MessagePackObject]
    public class Result<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        [Key(0)]
        public int code { get; set; }

        /// <summary>
        /// 业务状态码
        /// </summary>
        [Key(1)]
        public string subCode { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [Key(2)]
        public string message { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Key(3)]
        public DateTime operationTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 请求耗时
        /// </summary>
        [Key(4)]
        public long elapsedTime { get; set; } = -1;

        /// <summary>
        /// 数据
        /// </summary>
        [Key(5)]
        public T data { get; set; }
    }
}
