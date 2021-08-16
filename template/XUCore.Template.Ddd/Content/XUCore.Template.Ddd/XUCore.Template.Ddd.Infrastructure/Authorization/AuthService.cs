using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Cache;
using XUCore.Ddd.Domain.Bus;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.Template.Ddd.Domain.Auth.Permission;
using XUCore.Template.Ddd.Domain.User.User;
using XUCore.NetCore.Authorization.JwtBearer;

namespace XUCore.Template.Ddd.Infrastructure.Authorization
{
    public class AuthService : IAuthService
    {
        private const string userId = "__user_id__";
        private const string userName = "__user_name__";
        private const string loginToken = "__login_token__";
        private readonly ICacheManager cacheManager;
        private readonly IMediatorHandler bus;
        public AuthService(IServiceProvider serviceProvider)
        {
            bus = serviceProvider.GetService<IMediatorHandler>();
            cacheManager = serviceProvider.GetService<ICacheManager>();
        }

        public async Task<(string, string)> LoginAsync(UserLoginCommand command, CancellationToken cancellationToken = default)
        {
            var user = await bus.SendCommand(command, cancellationToken);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { userId , user.Id },
                { userName  ,user.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            SetLoginToken(user.Id, accessToken);

            return (accessToken, refreshToken);
        }

        public async Task LoginOutAsync(CancellationToken cancellationToken = default)
        {
            RemoveLoginToken();

            await Task.CompletedTask;
        }

        /// <summary>
        /// 将登录的用户写入内存作为标记，处理强制重新获取jwt，模拟退出登录（可以使用redis）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        private void SetLoginToken(string userId, string token)
        {
            cacheManager.Set($"{loginToken}{userId}", token);
        }
        /// <summary>
        /// 删除登录标记，模拟退出
        /// </summary>
        private void RemoveLoginToken()
        {
            cacheManager.Remove($"{loginToken}{UserId}");
        }
        /// <summary>
        /// 验证token是否一致
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool VaildLoginToken(string token)
        {
            var cacheToken = cacheManager.Get<string>($"{loginToken}{UserId}");

            return token == cacheToken;
        }

        public async Task<bool> IsCanAccessAsync(string accessKey)
        {
            return await bus.SendCommand(new PermissionQueryExists { UserId = UserId, OnlyCode = accessKey }, CancellationToken.None);
        }

        public bool IsAuthenticated => Identity.IsAuthenticated;

        private IIdentity Identity => Web.HttpContext.User.Identity;

        public string UserId => Identity.GetValue<string>(userId);

        public string UserName => Identity.GetValue<string>(userName);
    }
}
