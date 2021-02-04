using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using XUCore.NetCore.Middlewares;
using XUCore.Helpers;
using System;

namespace XUCore.NetCore.Extensions
{
    /// <summary>
    /// 中间件扩展
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// 注册错误日志中间件
        /// </summary>
        /// <param name="builder">应用程序生成器</param>
        public static IApplicationBuilder UseErrorLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLogMiddleware>();
        }

        /// <summary>
        /// 注册请求日志中间件
        /// </summary>
        /// <param name="builder">应用程序生成器</param>
        public static IApplicationBuilder UseRequestLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLogMiddleware>();
        }

        /// <summary>
        /// 启用静态请求上下文
        /// </summary>
        /// <param name="builder">应用程序生成器</param>
        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder builder)
        {
            Web.HttpContextAccessor = builder.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            Web.Environment = builder.ApplicationServices.GetService<IHostingEnvironment>();
            return builder;
        }

        /// <summary>
        /// 注册真实IP中间件
        /// </summary>
        /// <param name="builder">应用程序生成器</param>
        public static IApplicationBuilder UseRealIp(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RealIpMiddleware>();
        }

        /// <summary>
        /// 注册真实IP
        /// </summary>
        /// <param name="hostBuilder">Web主机生成器</param>
        /// <param name="headerKey">请求头键名</param>
        public static IWebHostBuilder UseRealIp(this IWebHostBuilder hostBuilder, string headerKey = "X-Forwarded-For")
        {
            if (hostBuilder == null)
            {
                throw new ArgumentNullException(nameof(hostBuilder));
            }

            if (hostBuilder.GetSetting(nameof(UseRealIp)) != null)
            {
                return hostBuilder;
            }

            hostBuilder.UseSetting(nameof(UseRealIp), true.ToString());

            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IStartupFilter>(new RealIpFilter());
                services.Configure<RealIpOptions>(options => options.HeaderKey = headerKey);
            });

            return hostBuilder;
        }
    }
}