using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Helpers;
using XUCore.NetCore;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.Swagger;
using XUCore.Serializer;
using XUCore.Template.Layer.Core;
using XUCore.Template.Layer.DbService.Admin.AdminUser;
using XUCore.Template.Layer.DbService.Admin.Permission;

namespace XUCore.Template.Layer.Applaction.Login
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    public class LoginAppService : AppService, ILoginAppService
    {
        private readonly IPermissionService permissionService;
        private readonly IAdminUserService adminUserService;
        private readonly IUserInfo user;

        public LoginAppService(IServiceProvider serviceProvider)
        {
            this.permissionService = serviceProvider.GetService<IPermissionService>();
            this.adminUserService = serviceProvider.GetService<IAdminUserService>();
            this.user = serviceProvider.GetService<IUserInfo>();
        }

        #region [ 登录 ]

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<LoginTokenDto>> LoginAsync(AdminUserLoginCommand command, CancellationToken cancellationToken)
        {
            var userDto = await adminUserService.LoginAsync(command, cancellationToken);

            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { ClaimAttributes.UserId , userDto.Id },
                { ClaimAttributes.UserName ,userDto.UserName }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            user.SetToken(userDto.Id.ToString(), accessToken);

            return RestFull.Success(data: new LoginTokenDto
            {
                Token = accessToken
            });
        }
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<string>> VerifyTokenAsync(CancellationToken cancellationToken)
        {
            return RestFull.Success(data: new
            {
                user.Id,
                user.UserName
            }.ToJson());
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task LoginOutAsync(CancellationToken cancellationToken)
        {
            user.RemoveToken();

            await Task.CompletedTask;
        }

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="onlyCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<bool>> GetPermissionExistsAsync(long adminId, string onlyCode, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.ExistsAsync(adminId, onlyCode, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionMenusAsync(long adminId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenusAsync(adminId, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionMenuExpressAsync(long adminId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenuExpressAsync(adminId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
