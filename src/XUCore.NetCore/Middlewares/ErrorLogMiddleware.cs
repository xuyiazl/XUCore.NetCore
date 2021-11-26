using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using XUCore.Extensions;
using System;
using System.Threading.Tasks;

namespace XUCore.NetCore.Middlewares
{
    /// <summary>
    /// 错误日志中间件
    /// </summary>
    public class ErrorLogMiddleware
    {
        /// <summary>
        /// 方法
        /// </summary>
        private readonly RequestDelegate _next;

        private readonly ILogger<ErrorLogMiddleware> _logger;

        /// <summary>
        /// 初始化一个<see cref="ErrorLogMiddleware"/>类型的实例
        /// </summary>
        /// <param name="next">方法</param>
        /// <param name="logger"></param>
        public ErrorLogMiddleware(RequestDelegate next, ILogger<ErrorLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                WriteLog(context, ex);
                throw;
            }
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="exception">异常</param>
        private void WriteLog(HttpContext context, Exception exception)
        {
            if (context == null)
                return;

            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogError(exception.FormatMessage($"全局异常捕获 - 错误日志中间件 - 状态码：{context.Response.StatusCode}"));
            }
        }
    }
}