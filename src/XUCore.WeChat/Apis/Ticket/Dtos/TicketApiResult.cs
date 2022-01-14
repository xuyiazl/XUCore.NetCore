using Newtonsoft.Json;
using System;

namespace XUCore.WeChat.Apis.Token
{
    public class TicketApiResult : ApiResultBase
    {
        /// <summary>
        /// 获取到的票据
        /// </summary>
        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        /// <summary>
        /// 有效时间，单位：秒
        /// </summary>
        [JsonProperty("expires_in")]
        internal int Expires { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiresTime { get; set; }
    }
}