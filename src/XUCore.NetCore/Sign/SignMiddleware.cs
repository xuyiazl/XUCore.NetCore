using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore.Middlewares;
using XUCore.Serializer;
using XUCore.Timing;

namespace XUCore.NetCore.Sign
{
    public class SignMiddleware : Middlewares.IMiddleware
    {
        /// <summary>
        /// 方法
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 签名选项
        /// </summary>
        private readonly SignOptions _options;

        /// <summary>
        /// 初始化一个<see cref="RealIpMiddleware"/>类型的实例
        /// </summary>
        /// <param name="next">方法</param>
        /// <param name="options">真实IP选项</param>
        public SignMiddleware(RequestDelegate next, IOptions<SignOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!_options.IsOpen)
            {
                await _next.Invoke(context);
                return;
            }

            var headers = context.Request.Headers;

            string appid = headers[$"{_options.Prefix}appid"].SafeString();
            string sign = headers[$"{_options.Prefix}sign"].SafeString();
            string timestamp = headers[$"{_options.Prefix}timestamp"].SafeString();
            string noncestr = headers[$"{_options.Prefix}noncestr"].SafeString();

            context.Response.ContentType = "application/json";

            if (appid.IsEmpty() || sign.IsEmpty() || timestamp.IsEmpty() || noncestr.IsEmpty())
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                await context.Response.WriteAsync(new Result<string>(StateCode.Fail, "1001", "缺少签名参数", "").ToJson(), Encoding.UTF8);

                return;
            }

            if (timestamp.Length != 10 || noncestr.Length != 16)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                await context.Response.WriteAsync(new Result<string>(StateCode.Fail, "1002", "签名参数不正确", "").ToJson(), Encoding.UTF8);

                return;
            }

            var nowTimestamp = DateTime.Now.ToTimeStamp(false);

            if (nowTimestamp - timestamp.ToLong() > _options.TimeOut)
            {
                context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;

                await context.Response.WriteAsync(new Result<string>(StateCode.Fail, "1003", "请求超时", "").ToJson(), Encoding.UTF8);

                return;
            }

            var appSecret = GetAppSecret(appid);

            var isVaildSign =
                SignParameters.Create()
                    .Add("appid", appid)
                    .Add("timestamp", timestamp)
                    .Add("noncestr", noncestr)
                    .VaildSign("key", appSecret, sign);

            if (!isVaildSign)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                await context.Response.WriteAsync(new Result<string>(StateCode.Fail, "1004", "签名错误", "").ToJson(), Encoding.UTF8);

                return;
            }

            if (!ReplayAttack(appid, timestamp, noncestr))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                await context.Response.WriteAsync(new Result<string>(StateCode.Fail, "1005", "请求失效", "").ToJson(), Encoding.UTF8);

                return;
            }

            await _next.Invoke(context);
        }
        /// <summary>
        /// 获取appSecret密钥
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public virtual string GetAppSecret(string appid) => "";
        /// <summary>
        /// 重放攻击
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <returns></returns>
        public virtual bool ReplayAttack(string appid, string timestamp, string noncestr) => true;
    }
}
