using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using XUCore.WeChat.Apis.Sns;
using XUCore.WeChat.AspNet;

namespace XUCore.WeChat.Net
{
    [AllowAnonymous]
    [WxPublicAccountOAuthFilter(OAuthLevel = OAuthLevels.OpenIdAndUserInfo)]
    public class WeChatPageModelBase : PageModel
    {
        /// <summary>
        /// 获取WebToken
        /// </summary>
        protected string WebAccessToken
        {
            get
            {
                var webAccessToken = Request.HttpContext.Items[WxConsts.COOKIE_WX_WEBTOKEN]?.ToString();
                if (string.IsNullOrEmpty(webAccessToken))
                {
                    webAccessToken = Request.Cookies[WxConsts.COOKIE_WX_WEBTOKEN];
                }

                return webAccessToken;
            }
        }

        /// <summary>
        /// 获取OpenId
        /// </summary>
        protected string OpenId
        {
            get
            {
                var openId = Request.HttpContext.Items[WxConsts.COOKIE_WX_OPENID]?.ToString();
                if (string.IsNullOrEmpty(openId))
                {
                    openId = Request.Cookies[WxConsts.COOKIE_WX_OPENID];
                }
                return openId;
            }
        }

        /// <summary>
        /// 获取微信用户信息
        /// </summary>
        /// <returns></returns>
        protected async Task<GetUserInfoApiResult> GetWeChatUserInfoAsync()
        {
            var snsApi =
               Request.HttpContext.RequestServices.GetRequiredService<ISnsApi>();
            var result = await snsApi.GetUserInfoAsync(WebAccessToken, OpenId);
            result.EnsureSuccess();
            return result;
        }
    }
}
