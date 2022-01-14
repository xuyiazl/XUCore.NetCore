using XUCore.WeChat.AspNet.Controllers;
using XUCore.WeChat.AspNet.ServerMessages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.WeChat.AspNet
{
    public static class Extentions
    {
        /// <summary>
        /// 添加服务器事件消息处理程序
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServerMessageHandler(this IServiceCollection services)
        {
            services.AddSingleton<ServerMessageHandler>();
            services.AddTransient<WxEventController>();
            return services;
        }

        /// <summary>
        /// 使用分布式缓存来存储AccessToken
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWxDistributedCacheForAccessToken(this IApplicationBuilder app)
        {
            var funcs = app.ApplicationServices.GetRequiredService<WxFuncs>();
            var cache = app.ApplicationServices.GetRequiredService<IDistributedCache>();

            if (funcs.GetTicketByAppId == null)
            {
                funcs.GetTicketByAppId = (appid) => cache.GetString($"{WxConsts.WX_PUBLICACCOUNT_CACHE_NAMESPACE}::JT::{appid}");
            }

            if (funcs.CacheTicket == null)
            {
                funcs.CacheTicket = (appid, ticket) =>
                {
                    if (string.IsNullOrEmpty(ticket))
                        throw new Exception("ticket 为空，请检查IP白名单，安全域名等设置");

                    byte[] tokenBytes = Encoding.UTF8.GetBytes(ticket);
                    cache.Set($"{WxConsts.WX_PUBLICACCOUNT_CACHE_NAMESPACE}::JT::{appid}", tokenBytes, new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(115)
                    });
                };
            }

            if (funcs.GetAccessTokenByAppId == null)
            {
                funcs.GetAccessTokenByAppId = (appid) => cache.GetString($"{WxConsts.WX_PUBLICACCOUNT_CACHE_NAMESPACE}::AT::{appid}");
            }

            if (funcs.CacheAccessToken == null)
            {
                funcs.CacheAccessToken = (appid, token) =>
                {
                    if (string.IsNullOrEmpty(token))
                        throw new Exception("access_token 为空，请检查IP白名单，安全域名等设置");

                    byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
                    cache.Set($"{WxConsts.WX_PUBLICACCOUNT_CACHE_NAMESPACE}::AT::{appid}", tokenBytes, new DistributedCacheEntryOptions()
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(115)
                    });
                };
            }
            return app;
        }
    }
}