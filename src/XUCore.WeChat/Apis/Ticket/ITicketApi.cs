using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebApiClientCore.Attributes;

namespace XUCore.WeChat.Apis.Token
{
    [HttpHost("https://api.weixin.qq.com/cgi-bin/ticket/")]
    public interface ITicketApi : IWxApiBase
    {
        /// <summary>
        /// 获取js_ticket票据
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("getticket")]
        Task<TicketApiResult> GetAsync([Required] string access_token, string type = "jsapi");
    }
}