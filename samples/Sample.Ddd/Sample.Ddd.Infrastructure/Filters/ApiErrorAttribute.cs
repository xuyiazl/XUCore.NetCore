using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.Helpers;
using Sample.Ddd.Domain.Core;
using XUCore.NetCore;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.Properties;

namespace Sample.Ddd.Infrastructure.Filters
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
                (var code, _) = SubCodeMessage.Message(SubCode.Cancel);

                context.ExceptionHandled = true;
                context.Result = new ObjectResult(new Result<object>(StateCode.Fail, R.CanceledMessage)
                {
                    SubCode = code
                })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                (var code, _) = SubCodeMessage.Message(SubCode.Unauthorized);

                context.Result = new ObjectResult(new Result<object>(StateCode.Fail, context.Exception.Message)
                {
                    SubCode = code
                })
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
            else if (context.Exception.IsFailure())
            {
                var ex = context.Exception as ValidationException;

                var message = ex.Failures.Select(c => c.Value.Join("")).Join("");

                (var code, _) = SubCodeMessage.Message(SubCode.ValidError);

                context.Result = new ObjectResult(new Result<object>()
                {
                    Code = 0,
                    SubCode = code,
                    Message = message,
                    Data = null,
                    ElapsedTime = -1,
                    OperationTime = DateTime.Now
                })
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<ApiErrorAttribute>>();

                if (logger.IsEnabled(LogLevel.Error))
                {
                    var routes = context.GetRouteValues().Select(c => $"{c.Key}={c.Value}").Join("，");

                    var str = new Str();
                    str.AppendLine("WebApi全局异常");
                    str.AppendLine($"路由信息：{routes}");
                    str.AppendLine($"IP：{context.HttpContext.Connection.RemoteIpAddress}");
                    str.AppendLine($"请求方法：{context.HttpContext.Request.Method}");
                    str.AppendLine($"请求地址：{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
                    logger.LogError(context.Exception.FormatMessage(str.ToString()));
                }

                (var code, _) = SubCodeMessage.Message(SubCode.Fail);

                context.Result = new ObjectResult(new Result<object>(StateCode.Fail, context.Exception.Message)
                {
                    SubCode = code
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

                base.OnException(context);
            }
        }
    }
}
