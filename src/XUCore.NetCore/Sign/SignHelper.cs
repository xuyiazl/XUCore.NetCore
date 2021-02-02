using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using XUCore.Helpers;
using XUCore.NetCore.HttpFactory;
using XUCore.Timing;

namespace XUCore.NetCore.Sign
{
    public static class SignHelper
    {
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
