using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Serializer;
using XUCore.Template.Layer.Applaction.Authorization;
using XUCore.Template.Layer.Core;
using XUCore.Template.Layer.DbService.Sys.Admin.AdminUser;
using XUCore.Template.Layer.DbService.Sys.Admin.Permission;

namespace XUCore.Template.Layer.Applaction.Login
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    public class LoginAppService : AppService, ILoginAppService
    {
        private readonly IPermissionService permissionService;
        private readonly IAuthService authService;

        public LoginAppService(IServiceProvider serviceProvider)
        {
            this.permissionService = serviceProvider.GetService<IPermissionService>();
            this.authService = serviceProvider.GetService<IAuthService>();
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
            (var accessToken, var refreshToken) = await authService.LoginAsync(command, cancellationToken);

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
                authService.AdminId,
                authService.AdminName
            }.ToJson());
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task LoginOutAsync(CancellationToken cancellationToken)
        {
            await authService.LoginOutAsync(cancellationToken);
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
