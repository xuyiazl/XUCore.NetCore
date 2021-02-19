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

namespace XUCore.NetCore.Signature
{
    /// <summary>
    /// http签名中间件
    /// <para>签名算法：</para>
    /// <para>1、拼接appid，timestamp，noncestr字符串，并按照字符进行ASCII排序，得到字符串：appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}</para>
    /// <para>2、将appSecret追加到拼接串后面，得到字符串：appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}&amp;key={appSecret}</para>
    /// <para>3、将期字符串MD5 32位 加密，然后全部小写 ToLower</para>
    /// <para>4、然后进行 Sha256(md5+appSecret).ToLower() 加密后，得到签名</para>
    /// <para>公式：Sha256(md5(appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}&amp;key={appSecret})+{appSecret})</para>
    /// </summary>
    public abstract class HttpSignMiddleware : Middlewares.IMiddleware
    {
        /// <summary>
        /// 方法
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// 签名选项
        /// </summary>
        private readonly HttpSignOptions _options;

        /// <summary>
        /// 初始化一个<see cref="HttpSignMiddleware"/>类型的实例
        /// <para>签名算法：</para>
        /// <para>1、拼接appid，timestamp，noncestr字符串，并按照字符进行ASCII排序，得到字符串：appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}</para>
        /// <para>2、将appSecret追加到拼接串后面，得到字符串：appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}&amp;key={appSecret}</para>
        /// <para>3、将期字符串MD5 32位 加密，然后全部小写 ToLower</para>
        /// <para>4、然后进行 Sha256(md5+appSecret).ToLower() 加密后，得到签名</para>
        /// <para>公式：Sha256(md5(appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}&amp;key={appSecret})+{appSecret})</para>
        /// </summary>
        /// <param name="next">方法</param>
        /// <param name="options">真实IP选项</param>
        public HttpSignMiddleware(RequestDelegate next, IOptions<HttpSignOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (!_options.IsOpen || path.IndexOf("/swagger") > -1)
            {
                await _next.Invoke(context);
                return;
            }

            var headers = context.Request.Headers;

            string appid = headers[$"{_options.Prefix}appid"].SafeString();
            string sign = headers[$"{_options.Prefix}sign"].SafeString();
            string timestamp = headers[$"{_options.Prefix}timestamp"].SafeString();
            string noncestr = headers[$"{_options.Prefix}noncestr"].SafeString();

            if (appid.IsEmpty() || sign.IsEmpty() || timestamp.IsEmpty() || noncestr.IsEmpty())
            {
                await WriteAsync(context, HttpSignSubCode.MissingParams);

                return;
            }

            if (timestamp.Length != 10 || noncestr.Length != 16)
            {
                await WriteAsync(context, HttpSignSubCode.ParamsFail);

                return;
            }

            var nowTimestamp = DateTime.Now.ToTimeStamp(false);

            if (nowTimestamp - timestamp.ToLong() > _options.TimeOut)
            {
                await WriteAsync(context, HttpSignSubCode.RequestTimeout);

                return;
            }

            var appSecret = await GetAppSecretAsync(appid);

            if (!(await CreateSignAsync(appid, appSecret, timestamp, noncestr)).Equals(sign))
            {
                await WriteAsync(context, HttpSignSubCode.SignFail);

                return;
            }

            if (!(await PreventReplayAttackAsync(appid, timestamp, noncestr)))
            {
                await WriteAsync(context, HttpSignSubCode.RequestFail);

                return;
            }

            await _next.Invoke(context);
        }
        /// <summary>
        /// 获取appSecret密钥
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public abstract Task<string> GetAppSecretAsync(string appid);
        /// <summary>
        /// 防止重放攻击
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <returns></returns>
        public virtual async Task<bool> PreventReplayAttackAsync(string appid, string timestamp, string noncestr) => await Task.FromResult(true);
        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appSecret"></param>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <returns></returns>
        public virtual async Task<string> CreateSignAsync(string appid, string appSecret, string timestamp, string noncestr)
        {
            await Task.CompletedTask;

            return
                SignParameters.Create()
                    .Add("appid", appid)
                    .Add("timestamp", timestamp)
                    .Add("noncestr", noncestr)
                    .CreateSign("key", appSecret);
        }
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="subCode"></param>
        /// <returns></returns>
        private async Task WriteAsync(HttpContext context, HttpSignSubCode subCode)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.OK;

            var (code, message) = HttpSignCode.Message(subCode);

            await context.Response.WriteAsync(new Result<string>(StateCode.Fail, code, message, "").ToJson(), Encoding.UTF8);
        }
    }
}
