using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Swagger;
using XUCore.WebApi2.Template.DbService.Sys.Admin.AdminUser;
using XUCore.WebApi2.Template.DbService.Sys.Admin.Permission;

namespace XUCore.WebApi2.Template.Applaction.Authorization
{
    /// <summary>
    /// 管理员服务
    /// </summary>
    [NonDynamicWebApi]
    public class AuthService : IAuthService
    {
        private const string userId = "_admin_userid";
        private const string userName = "_admin_username";

        private readonly IAdminUserService adminUserService;
        private readonly IPermissionService permissionService;
        public AuthService(IServiceProvider serviceProvider)
        {
            adminUserService = serviceProvider.GetService<IAdminUserService>();
            permissionService = serviceProvider.GetService<IPermissionService>();
        }

        public async Task<(string, string)> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var user = await adminUserService.LoginAsync(request, cancellationToken);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { userId , user.Id},
                { userName  ,user.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            return (accessToken, refreshToken);
        }

        public async Task LoginOutAsync()
        {
            await Task.CompletedTask;
        }

        public bool IsCanAccess(string accessKey)
        {
            if (!IsAuthenticated)
                return false;
            if (!string.IsNullOrWhiteSpace(accessKey))
                return permissionService.ExistsAsync(AdminId, accessKey, CancellationToken.None).Result;
            return true;
        }

        public bool IsAuthenticated => Identity.IsAuthenticated;

        private IIdentity Identity => Web.HttpContext.User.Identity;

        public long AdminId => Identity.GetValue<long>(userId);

        public string AdminName => Identity.GetValue<string>(userName);
    }
}
