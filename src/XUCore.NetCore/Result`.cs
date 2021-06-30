using MessagePack;
using Microsoft.AspNetCore.Mvc;
using XUCore.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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
        /// 返回结构体
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public Result(StateCode code, string message, T data = default) : this(code.Value(), "", message, data)
        {
        }
        /// <summary>
        /// 返回结构体
        /// </summary>
        /// <param name="code"></param>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public Result(StateCode code, string subCode, string message, T data = default) : this(code.Value(), subCode, message, data)
        {
        }
        /// <summary>
        /// 返回结构体
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public Result(int code, string message, T data = default) : this(code, "", message, data)
        {
        }
        /// <summary>
        /// 返回结构体
        /// </summary>
        /// <param name="code"></param>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public Result(int code, string subCode, string message, T data = default)
        {
            this.Code = code;
            this.SubCode = subCode;
            this.Message = message;
            this.Data = data;
        }
        public Result()
        {

        }
        /// <summary>
        /// 状态码
        /// </summary>
        [Key(0)]
        public int Code { get; set; }

        /// <summary>
        /// 业务状态码
        /// </summary>
        [Key(1)]
        public string SubCode { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [Key(2)]
        public string Message { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        [Key(3)]
        public DateTime OperationTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 请求耗时
        /// </summary>
        [Key(4)]
        public long ElapsedTime { get; set; } = -1;

        /// <summary>
        /// 数据
        /// </summary>
        [Key(5)]
        public T Data { get; set; }
    }
}
