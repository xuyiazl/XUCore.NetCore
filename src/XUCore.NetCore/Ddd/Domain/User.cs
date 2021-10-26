using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using XUCore.Cache;
using XUCore.Extensions;
using XUCore.Helpers;

namespace XUCore.Ddd.Domain
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
        /// IIdentity
        /// </summary>
        protected IIdentity Identity => accessor?.HttpContext?.User?.Identity;
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual string Id => Identity?.GetValue<string>(ClaimAttributes.UserId);
        /// <summary>
        /// 用户Id转换类型
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        public virtual TKey GetId<TKey>() => Conv.To<TKey>(Id);
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName => Identity?.GetValue<string>(ClaimAttributes.UserName);
        /// <summary>
        /// 昵称
        /// </summary>
        public virtual string NickName => Identity?.GetValue<string>(ClaimAttributes.UserNickName);

        /// <summary>
        /// 将登录的用户写入内存作为标记，处理强制重新获取jwt，模拟退出登录（可以使用redis）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        public virtual void SetToken(string id, string token)
        {
            cacheManager.Set($"{ClaimAttributes.UserToken}_{Id}", token);
        }
        /// <summary>
        /// 删除登录标记，模拟退出
        /// </summary>
        public virtual void RemoveToken()
        {
            cacheManager.Remove($"{ClaimAttributes.UserToken}_{Id}");
        }
        /// <summary>
        /// 验证token是否一致
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public virtual bool VaildToken(string token)
        {
            var cacheToken = cacheManager.Get<string>($"{ClaimAttributes.UserToken}_{Id}");

            return token == cacheToken;
        }
    }
}
