using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.Properties;
using XUCore.Extensions;
using XUCore.Helpers;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Linq;

namespace XUCore.NetCore.Filters
{
    /// <summary>
    /// API错误日志过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context">异常上下文</param>
        public override void OnException(ExceptionContext context)
        {
            if (context == null)
                return;

            if (context.Exception is OperationCanceledException)
            {
                //过滤掉客户端取消请求的异常，属于正常异常范围
                //_logger.LogInformation("Request was cancelled");
                context.ExceptionHandled = true;
                //context.Result = new Result(StateCode.Fail, "", R.CanceledMessage);
                context.Result = new ObjectResult(new Result<string>(StateCode.Fail, R.CanceledMessage))
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                //context.Result = new Result(StateCode.Fail, "", context.Exception.Message);

                context.Result = new ObjectResult(new Result<string>(StateCode.Fail, context.Exception.Message))
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
            else
            {
                var logger = Web.GetService<ILogger<ApiErrorAttribute>>();

                if (logger.IsEnabled(LogLevel.Error))
                {
                    //var areaName = context.GetAreaName();
                    //var controllerName = context.GetControllerName();
                    var routes = context.GetRouteValues().Select(c => $"{c.Key}={c.Value}").Join("，");

                    var str = new Str();
                    str.AppendLine("WebApi全局异常");
                    str.AppendLine($"路由信息：{routes}");
                    str.AppendLine($"IP：{context.HttpContext.Connection.RemoteIpAddress}");
                    str.AppendLine($"请求方法：{context.HttpContext.Request.Method}");
                    str.AppendLine($"请求地址：{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
                    logger.LogError(context.Exception.FormatMessage(str.ToString()));
                }

                //context.Result = new Result(StateCode.Fail, "", R.SystemError);

                context.Result = new ObjectResult(new Result<string>(StateCode.Fail, context.Exception.Message))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

                //context.Result = new RedirectResult(_appSettings.CustomErrorPage);

                //EmailHelper.SendMail(exception);//发送邮件通知到相关人员

                base.OnException(context);
            }
        }
    }
}