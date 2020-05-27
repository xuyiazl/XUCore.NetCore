using Microsoft.AspNetCore.Mvc;
using XUCore.Extensions;
using System;
using System.Threading.Tasks;

namespace XUCore.NetCore
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result : JsonResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; }

        /// <summary>
        /// 业务状态码
        /// </summary>
        public string subCode { get; }

        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; }

        /// <summary>
        /// 数据
        /// </summary>
        public dynamic data { get; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime operationTime { get; }

        /// <summary>
        /// 请求耗时
        /// </summary>
        public long elapsedTime { get; set; }

        /// <summary>
        /// 初始化一个<see cref="Result"/>类型的实例
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="subCode">业务状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public Result(int code, string subCode, string message, dynamic data = null) : base(null)
        {
            this.code = code;
            this.subCode = subCode;
            this.message = message;
            this.data = data;
            this.operationTime = DateTime.Now;
            this.elapsedTime = -1;
        }

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="subCode">业务状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public Result(StateCode code, string subCode, string message, dynamic data = null) : base(null)
        {
            this.code = code.Value();
            this.subCode = subCode;
            this.message = message;
            this.data = data;
            this.operationTime = DateTime.Now;
            this.elapsedTime = -1;
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            this.Value = new
            {
                code,
                subCode,
                message,
                elapsedTime,
                operationTime,
                data
            };
            return base.ExecuteResultAsync(context);
        }
    }
}