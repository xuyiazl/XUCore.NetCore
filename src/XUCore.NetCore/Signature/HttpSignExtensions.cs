using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using XUCore.Configs;
using XUCore.Helpers;
using XUCore.NetCore.HttpFactory;
using XUCore.Timing;

namespace XUCore.NetCore.Signature
{
    public static class HttpSignExtensions
    {
        /// <summary>
        /// 注册签名服务，<see cref="HttpSignOptions"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpSignService(this IServiceCollection services, string section = "HttpSignOptions")
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.BindSection<HttpSignOptions>(configuration, section);

            return services;
        }
        /// <summary>
        /// 注册签名中间件，<see cref="HttpSignMiddleware"/>
        /// </summary>
        /// <param name="builder">应用程序生成器</param>
        public static IApplicationBuilder UseHttpSign<TMiddleware>(this IApplicationBuilder builder)
            where TMiddleware : HttpSignMiddleware
        {
            return builder.UseMiddleware<TMiddleware>();
        }
        /// <summary>
        /// 写入签名
        /// </summary>
        /// <param name="client"></param>
        /// <param name="appId">应用id</param>
        /// <param name="appSecret">密钥</param>
        /// <param name="prefix">header 前缀</param>
        public static void SetSignature(this HttpClient client, string appId, string appSecret, string prefix = "x-client-")
        {
            var timestamp = DateTime.Now.ToTimeStamp(false);

            var noncestr = Str.GetNonceStr(16, isCharacter: false);

            var sign =
                SignParameters.Create()
                    .Add("appid", appId)
                    .Add("timestamp", timestamp.ToString())
                    .Add("noncestr", noncestr)
                    .CreateSign("key", appSecret);

            client.SetHeader($"{prefix}appid", appId);
            client.SetHeader($"{prefix}sign", sign);
            client.SetHeader($"{prefix}timestamp", timestamp.ToString());
            client.SetHeader($"{prefix}noncestr", noncestr);
        }
    }
}
