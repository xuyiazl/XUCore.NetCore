using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Timing;

namespace XUCore.NetCore.Signature
{
    /// <summary>
    /// http签名验证
    /// <para>签名算法：</para>
    /// <para>1、拼接appid，timestamp，noncestr字符串，并按照字符进行ASCII排序，得到字符串：appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}</para>
    /// <para>2、将appSecret追加到拼接串后面，得到字符串：appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}&amp;key={appSecret}</para>
    /// <para>3、将期字符串MD5 32位 加密，然后全部小写 ToLower</para>
    /// <para>4、然后进行 Sha256(md5+appSecret).ToLower() 加密后，得到签名</para>
    /// <para>公式：Sha256(md5(appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}&amp;key={appSecret})+{appSecret})</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class HttpSignAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// http签名验证
        /// <para>签名算法：</para>
        /// <para>1、拼接appid，timestamp，noncestr字符串，并按照字符进行ASCII排序，得到字符串：appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}</para>
        /// <para>2、将appSecret追加到拼接串后面，得到字符串：appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}&amp;key={appSecret}</para>
        /// <para>3、将期字符串MD5 32位 加密，然后全部小写 ToLower</para>
        /// <para>4、然后进行 Sha256(md5+appSecret).ToLower() 加密后，得到签名</para>
        /// <para>公式：Sha256(md5(appid={appid}&amp;timestamp={10位时间戳}&amp;noncestr={16位随机字符串}&amp;key={appSecret})+{appSecret})</para>
        /// </summary>
        public HttpSignAttribute()
        {

        }
        /// <summary>
        /// header前缀，默认x-client-
        /// </summary>
        public string Prefix { get; set; } = "x-client-";
        /// <summary>
        /// 超时设置（秒），默认60秒
        /// </summary>
        public int Timeout { get; set; } = 60;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var serviceProvider = context.HttpContext.RequestServices;

            var headers = context.HttpContext.Request.Headers;

            string appid = headers[$"{Prefix}appid"].SafeString();
            string sign = headers[$"{Prefix}sign"].SafeString();
            string timestamp = headers[$"{Prefix}timestamp"].SafeString();
            string noncestr = headers[$"{Prefix}noncestr"].SafeString();

            if (appid.IsEmpty() || sign.IsEmpty() || timestamp.IsEmpty() || noncestr.IsEmpty())
            {
                Result(context, HttpSignSubCode.MissingParams);
                return;
            }

            if (timestamp.Length != 10 || noncestr.Length != 16)
            {
                Result(context, HttpSignSubCode.ParamsFail);
                return;
            }

            var nowTimestamp = DateTime.Now.ToTimeStamp(false);

            if (nowTimestamp - timestamp.ToLong() > Timeout)
            {
                Result(context, HttpSignSubCode.RequestTimeout);
                return;
            }

            var appSecret = GetAppSecretAsync(serviceProvider, appid).GetAwaiter().GetResult();

            if (!CreateSignAsync(serviceProvider, appid, appSecret, timestamp, noncestr).GetAwaiter().GetResult().Equals(sign))
            {
                Result(context, HttpSignSubCode.SignFail);
                return;
            }

            if (!PreventReplayAttackAsync(serviceProvider, appid, timestamp, noncestr).GetAwaiter().GetResult())
            {
                Result(context, HttpSignSubCode.RequestFail);
                return;
            }

            base.OnActionExecuting(context);
        }
        /// <summary>
        /// 获取appSecret密钥
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public abstract Task<string> GetAppSecretAsync(IServiceProvider serviceProvider, string appid);
        /// <summary>
        /// 防止重放攻击
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="appid"></param>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <returns></returns>
        public virtual async Task<bool> PreventReplayAttackAsync(IServiceProvider serviceProvider, string appid, string timestamp, string noncestr) => await Task.FromResult(true);
        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="appid"></param>
        /// <param name="appSecret"></param>
        /// <param name="timestamp"></param>
        /// <param name="noncestr"></param>
        /// <returns></returns>
        public virtual async Task<string> CreateSignAsync(IServiceProvider serviceProvider, string appid, string appSecret, string timestamp, string noncestr)
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
        private void Result(ActionExecutingContext context, HttpSignSubCode subCode)
        {
            var (code, message) = HttpSignCode.Message(subCode);

            context.Result = new Result(StateCode.Fail, code, message, "");
        }
    }
}
