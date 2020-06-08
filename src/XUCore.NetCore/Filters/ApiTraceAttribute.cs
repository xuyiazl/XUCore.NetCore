using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.IO;
using XUCore.Serializer;
using System;
using System.Threading.Tasks;

namespace XUCore.NetCore.Filters
{
    /// <summary>
    /// API跟踪日志过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiTraceAttribute : ActionFilterAttribute
    {
        public ApiTraceAttribute()
        {
        }

        /// <summary>
        /// 是否忽略，为true不记录日志
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// 获取日志操作
        /// </summary>
        /// <returns></returns>
        private ILogger _logger { get; set; }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context">操作执行上下文</param>
        /// <param name="next">委托</param>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (next == null)
                throw new ArgumentNullException(nameof(next));

            _logger = Web.GetService<ILogger<ApiErrorAttribute>>();

            Str logString = new Str();
            OnActionExecuting(context);
            await OnActionExecutingAsync(context, logString);
            if (context.Result != null)
                return;
            var executedContext = await next();
            OnActionExecuted(executedContext);
            logString.Clear();
            OnActionExecuted(executedContext, logString);
        }

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="context">操作执行上下文</param>
        /// <param name="log">日志</param>
        protected async Task OnActionExecutingAsync(ActionExecutingContext context, Str log)
        {
            if (Ignore) return;

            if (!_logger.IsEnabled(LogLevel.Trace)) return;

            await WriteLogAsync(context, log);
        }

        /// <summary>
        /// 执行前日志
        /// </summary>
        /// <param name="context">操作执行上下文</param>
        /// <param name="log">日志</param>
        private async Task WriteLogAsync(ActionExecutingContext context, Str log)
        {
            log.AppendLine("WebApi跟踪-准备执行操作")
                .AppendLine(context.Controller.SafeString())
                .AppendLine(context.ActionDescriptor.DisplayName)
                .AppendLine($"【客户端IP】{context.HttpContext.Connection.RemoteIpAddress.ToString()}")
                .AppendLine($"【请求地址】{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");

            await AddRequestInfoAsync(context, log);
            _logger.LogTrace(log.ToString());
        }

        /// <summary>
        /// 添加请求信息参数
        /// </summary>
        /// <param name="context">操作执行上下文</param>
        /// <param name="log">日志</param>
        private async Task AddRequestInfoAsync(ActionExecutingContext context, Str log)
        {
            var request = context.HttpContext.Request;
            log.AppendLine($"【Http请求方式】{request.Method}");
            if (string.IsNullOrWhiteSpace(request.ContentType) == false)
                log.AppendLine($"【ContentType】{request.ContentType}");
            AddHeaders(request, log);
            await AddFormParamsAsync(request, log);
            AddCookie(request, log);
        }

        /// <summary>
        /// 添加请求头
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <param name="log">日志</param>
        private void AddHeaders(Microsoft.AspNetCore.Http.HttpRequest request, Str log)
        {
            if (request.Headers == null || request.Headers.Count == 0)
                return;
            log.AppendLine("【Headers】");
            log.AppendLine(JsonHelper.ToJson(request.Headers));
        }

        /// <summary>
        /// 添加表单参数
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <param name="log">日志</param>
        private async Task AddFormParamsAsync(Microsoft.AspNetCore.Http.HttpRequest request, Str log)
        {
            if (IsMultipart(request.ContentType))
                return;
            //request.EnableRewind();
            var result = await FileHelper.ToStringAsync(request.Body, isCloseStream: false);
            if (string.IsNullOrWhiteSpace(result))
                return;
            log.AppendLine("【表单参数】");
            log.AppendLine(result);
        }

        /// <summary>
        /// 是否multipart内容类型
        /// </summary>
        /// <param name="contentType">内容类型</param>
        private static bool IsMultipart(string contentType) =>
            !string.IsNullOrEmpty(contentType) &&
            contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <param name="log">日志</param>
        private void AddCookie(Microsoft.AspNetCore.Http.HttpRequest request, Str log)
        {
            log.AppendLine("【Cookie】");
            foreach (var key in request.Cookies.Keys)
                log.AppendLine($"{key}:{request.Cookies[key]}");
        }

        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="context">操作执行上下文</param>
        /// <param name="log">日志</param>
        public virtual void OnActionExecuted(ActionExecutedContext context, Str log)
        {
            if (Ignore) return;

            if (!_logger.IsEnabled(LogLevel.Trace)) return;

            WriteLog(context, log);
        }

        /// <summary>
        /// 执行后的日志
        /// </summary>
        /// <param name="context">操作执行上下文</param>
        /// <param name="log">日志</param>
        private void WriteLog(ActionExecutedContext context, Str log)
        {
            log.AppendLine("WebApi跟踪-执行操作完成")
                .AppendLine(context.Controller.SafeString())
                .AppendLine(context.ActionDescriptor.DisplayName);
            AddResponseInfo(context, log);
            AddResult(context, log);
            _logger.LogTrace(log.ToString());
        }

        /// <summary>
        /// 添加响应信息参数
        /// </summary>
        /// <param name="context">操作执行上下文</param>
        /// <param name="log">日志</param>
        private void AddResponseInfo(ActionExecutedContext context, Str log)
        {
            var response = context.HttpContext.Response;
            if (string.IsNullOrWhiteSpace(response.ContentType) == false)
                log.AppendLine($"【ContentType】{response.ContentType}");
            log.AppendLine($"【Http状态码】{response.StatusCode}");
        }

        /// <summary>
        /// 记录响应结果
        /// </summary>
        /// <param name="context">操作执行上下文</param>
        /// <param name="log">日志</param>
        private void AddResult(ActionExecutedContext context, Str log)
        {
            if (!(context.Result is Result result))
                return;
            log.AppendLine($"【响应消息】{result.message}")
                .AppendLine("【响应结果】")
                .AppendLine(JsonHelper.ToJson(result.data));
        }
    }
}