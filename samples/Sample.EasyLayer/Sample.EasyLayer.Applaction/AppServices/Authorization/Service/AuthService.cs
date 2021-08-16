using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Cache;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Swagger;
using Sample.EasyLayer.DbService.Sys.Admin.AdminUser;
using Sample.EasyLayer.DbService.Sys.Admin.Permission;

namespace Sample.EasyLayer.Applaction.Authorization
{
    /// <summary>
    /// 管理员服务
    /// </summary>
    [NonDynamicWebApi]
    public class AuthService : IAuthService
    {
        private const string userId = "_admin_userid";
        private const string userName = "_admin_username";
        private const string loginToken = "_admin_login_";

        private readonly IAdminUserService adminUserService;
        private readonly IPermissionService permissionService;
        private readonly ICacheManager cacheManager;
        public AuthService(IServiceProvider serviceProvider)
        {
            adminUserService = serviceProvider.GetService<IAdminUserService>();
            permissionService = serviceProvider.GetService<IPermissionService>();
            cacheManager = serviceProvider.GetService<ICacheManager>();
        }

        public async Task<(string, string)> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var user = await adminUserService.LoginAsync(request, cancellationToken);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { userId , user.Id },
                { userName  ,user.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

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
        /// <param name="adminId"></param>
        /// <param name="token"></param>
        private void SetLoginToken(long adminId, string token)
        {
            cacheManager.Set($"{loginToken}{adminId}", token);
        }
        /// <summary>
        /// 删除登录标记，模拟退出
        /// </summary>
        private void RemoveLoginToken()
        {
            cacheManager.Remove($"{loginToken}{AdminId}");
        }
        /// <summary>
        /// 验证token是否一致
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool VaildLoginToken(string token)
        {
            var cacheToken = cacheManager.Get<string>($"{loginToken}{AdminId}");

            return token == cacheToken;
        }

        public async Task<bool> IsCanAccessAsync(string accessKey)
        {
            return await permissionService.ExistsAsync(AdminId, accessKey, CancellationToken.None);
        }

        public bool IsAuthenticated => Identity.IsAuthenticated;

        private IIdentity Identity => Web.HttpContext.User.Identity;

        public long AdminId => Identity.GetValue<long>(userId);

        public string AdminName => Identity.GetValue<string>(userName);
    }
}
