using XUCore.WeChat.Apis.Token;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using XUCore.WeChat.Helper;
using XUCore.Helpers;

namespace XUCore.WeChat
{
    /// <summary>
    /// 公众号AccessToken管理器
    /// </summary>
    public class TokenManager : ITokenManager
    {
        private readonly WxFuncs weChatFuncs;
        private readonly IServiceProvider serviceProvider;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weChatFuncs"></param>
        /// <param name="serviceProvider"></param>
        public TokenManager(WxFuncs weChatFuncs, IServiceProvider serviceProvider)
        {
            this.weChatFuncs = weChatFuncs;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <returns></returns>
        public virtual async Task<string> GetAccessTokenAsync()
        {
            WxPublicAccountOption options = weChatFuncs?.GetWeChatOptions();
            string token = weChatFuncs?.GetAccessTokenByAppId(options?.AppId);
            if (string.IsNullOrEmpty(token))
            {
                ITokenApi tokenApi = serviceProvider.GetService<ITokenApi>();
                TokenApiResult result = await tokenApi.GetAsync(options.AppId, options.AppSecret);
                weChatFuncs?.CacheAccessToken(options.AppId, result.AccessToken);
                return result.AccessToken;
            }
            return token;
        }
    }


    /// <summary>
    /// 公众号js ticket管理器
    /// </summary>
    public class TicketManager : ITicketManager
    {
        private readonly WxFuncs weChatFuncs;
        private readonly IServiceProvider serviceProvider;
        private readonly ITokenManager tokenManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weChatFuncs"></param>
        /// <param name="tokenManager"></param>
        /// <param name="serviceProvider"></param>
        public TicketManager(WxFuncs weChatFuncs, ITokenManager tokenManager, IServiceProvider serviceProvider)
        {
            this.weChatFuncs = weChatFuncs;
            this.serviceProvider = serviceProvider;
            this.tokenManager = tokenManager;
        }

        /// <summary>
        /// 获取ticket
        /// </summary>
        /// <returns></returns>
        public virtual async Task<string> GetTicketAsync()
        {
            WxPublicAccountOption options = weChatFuncs?.GetWeChatOptions();
            string ticket = weChatFuncs?.GetTicketByAppId(options?.AppId);

            if (string.IsNullOrEmpty(ticket))
            {
                var accessToken = await tokenManager.GetAccessTokenAsync();

                var ticketApi = serviceProvider.GetService<ITicketApi>();
                var result = await ticketApi.GetAsync(accessToken);
                weChatFuncs?.CacheTicket(options.AppId, result.Ticket);
                return result.Ticket;
            }

            return ticket;
        }
    }

    /// <summary>
    /// 分享数据参数
    /// </summary>
    public class WebShareSignatureResult
    {
        /// <summary>
        /// appid
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 随机数字
        /// </summary>
        public string Nonce { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public long TimeStamp { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
    }

    public class WebShareSignature : IWebShareSignature
    {
        private readonly WxFuncs weChatFuncs;
        private readonly ITicketManager ticketManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weChatFuncs"></param>
        /// <param name="ticketManager"></param>
        public WebShareSignature(WxFuncs weChatFuncs, ITicketManager ticketManager)
        {
            this.weChatFuncs = weChatFuncs;
            this.ticketManager = ticketManager;
        }

        /// <summary>
        /// 获取网页授权签名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<WebShareSignatureResult> Create(string url)
        {
            WxPublicAccountOption options = weChatFuncs?.GetWeChatOptions();

            var ticket = await ticketManager.GetTicketAsync();

            var nonce = Str.GetNonceStr(16, isCharacter: false);

            var timestamp = DateTime.Now.ConvertToTimeStamp();

            var str = new StringBuilder();
            str.AppendFormat("jsapi_ticket={0}&", ticket);
            str.AppendFormat("noncestr={0}&", nonce);
            str.AppendFormat("timestamp={0}&", timestamp);
            str.AppendFormat("url={0}", url);

            using var sha = SHA1.Create();
            var shaBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(str.ToString()));
            string sign = BitConverter.ToString(shaBytes).Replace("-", "").ToLower();

            return new()
            {
                AppId = options.AppId,
                Nonce = nonce,
                TimeStamp = timestamp,
                Signature = sign
            };
        }
    }
}
