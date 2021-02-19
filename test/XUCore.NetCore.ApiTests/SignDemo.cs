using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XUCore.NetCore.Signature;

namespace XUCore.NetCore.ApiTests
{
    public class SignMiddlewareDemo : HttpSignMiddleware
    {
        public SignMiddlewareDemo(RequestDelegate next, IOptions<HttpSignOptions> options)
            : base(next, options)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public override Task<string> GetAppSecretAsync(string appid)
        {
            appid = "web1ed21e4udroo37fmj";

            return Task.FromResult("CdzL5v9s6cmYOqeYW2ZicfdTaT3LdXhJ");
        }
        /// <summary>
        /// 重放处理
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <returns></returns>
        public override Task<bool> PreventReplayAttackAsync(string appid, string timestamp, string noncestr)
        {
            return base.PreventReplayAttackAsync(appid, timestamp, noncestr);
        }
    }

    public class HttpSignApiAttribute : HttpSignAttribute
    {
        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public override Task<string> GetAppSecretAsync(IServiceProvider serviceProvider, string appid)
        {
            appid = "web1ed21e4udroo37fmj";

            return Task.FromResult("CdzL5v9s6cmYOqeYW2ZicfdTaT3LdXhJ");
        }
        /// <summary>
        /// 重放处理
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="appid"></param>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <returns></returns>
        public override Task<bool> PreventReplayAttackAsync(IServiceProvider serviceProvider, string appid, string timestamp, string noncestr)
        {
            return base.PreventReplayAttackAsync(serviceProvider, appid, timestamp, noncestr);
        }
    }
}
