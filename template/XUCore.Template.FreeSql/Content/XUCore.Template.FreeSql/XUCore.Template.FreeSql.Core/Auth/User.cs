using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using XUCore.Cache;
using XUCore.Extensions;

namespace XUCore.Template.FreeSql.Core.Auth
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User : IUser
    {
        private readonly IHttpContextAccessor accessor;
        private readonly ICacheManager cacheManager;

        public User(IServiceProvider serviceProvider)
        {
            accessor = serviceProvider.GetService<IHttpContextAccessor>();
            cacheManager = serviceProvider.GetService<ICacheManager>();
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual long Id
        {
            get
            {
                var id = accessor?.HttpContext?.User?.Identity.GetValue<long>(ClaimAttributes.UserId);
                if (!id.IsNull())
                    return id.Value;
                return 0;
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName => accessor?.HttpContext?.User?.Identity.GetValue<string>(ClaimAttributes.UserName);
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName => accessor?.HttpContext?.User?.Identity.GetValue<string>(ClaimAttributes.UserNickName);

        /// <summary>
        /// 将登录的用户写入内存作为标记，处理强制重新获取jwt，模拟退出登录（可以使用redis）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        public virtual void SetLoginToken(long userId, string token)
        {
            cacheManager.Set($"{ClaimAttributes.UserToken}{userId}", token);
        }
        /// <summary>
        /// 删除登录标记，模拟退出
        /// </summary>
        public virtual void RemoveLoginToken()
        {
            cacheManager.Remove($"{ClaimAttributes.UserToken}{Id}");
        }
        /// <summary>
        /// 验证token是否一致
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual bool VaildLoginToken(string token)
        {
            var cacheToken = cacheManager.Get<string>($"{ClaimAttributes.UserToken}{Id}");

            return token == cacheToken;
        }
    }
}
