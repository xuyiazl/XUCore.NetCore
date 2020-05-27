using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace XUCore.NetCore.Middlewares
{
    /// <summary>
    /// 真实IP中间件
    /// </summary>
    public class RealIpMiddleware : IMiddleware
    {
        /// <summary>
        /// 方法
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 真实IP选项
        /// </summary>
        private readonly RealIpOptions _options;

        /// <summary>
        /// 初始化一个<see cref="RealIpMiddleware"/>类型的实例
        /// </summary>
        /// <param name="next">方法</param>
        /// <param name="options">真实IP选项</param>
        public RealIpMiddleware(RequestDelegate next, IOptions<RealIpOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            try
            {
                if (headers.ContainsKey(_options.HeaderKey))
                {
                    context.Connection.RemoteIpAddress = IPAddress.Parse(
                        _options.HeaderKey.ToLower() == "x-forwarded-for"
                            ? headers["X-Forwarded-For"].ToString().Split(',')[0]
                            : headers[_options.HeaderKey].ToString());
                }
            }
            finally
            {
                await _next(context);
            }
        }
    }

    /// <summary>
    /// 真实IP选项
    /// </summary>
    public class RealIpOptions
    {
        /// <summary>
        /// 请求头键名
        /// </summary>
        public string HeaderKey { get; set; }
    }

    /// <summary>
    /// 真实IP过滤器
    /// </summary>
    public class RealIpFilter : IStartupFilter
    {
        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="next">方法</param>
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) => app =>
        {
            app.UseMiddleware<RealIpMiddleware>();
            next(app);
        };
    }
}