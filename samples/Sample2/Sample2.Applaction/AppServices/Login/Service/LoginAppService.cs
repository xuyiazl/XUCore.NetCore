using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Serializer;
using Sample2.Applaction.Authorization;
using Sample2.Core;
using Sample2.DbService.Sys.Admin.AdminUser;
using Sample2.DbService.Sys.Admin.LoginRecord;
using Sample2.DbService.Sys.Admin.Permission;

namespace Sample2.Applaction.Login
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class LoginAppService : AppService, ILoginAppService
    {
        private readonly IPermissionService permissionService;
        private readonly ILoginRecordService loginRecordService;
        private readonly IAuthService authService;

        public LoginAppService(IServiceProvider serviceProvider)
        {
            this.permissionService = serviceProvider.GetService<IPermissionService>();
            this.loginRecordService = serviceProvider.GetService<ILoginRecordService>();
            this.authService = serviceProvider.GetService<IAuthService>();
        }

        #region [ 登录 ]

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<Result<LoginTokenDto>> LoginAsync([Required][FromBody] AdminUserLoginCommand request, CancellationToken cancellationToken)
        {
            request.IsVaild();

            (var accessToken, var refreshToken) = await authService.LoginAsync(request, cancellationToken);

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
        [HttpGet]
        public async Task<Result<string>> VerifyTokenAsync(CancellationToken cancellationToken)
        {
            return RestFull.Success(data: new
            {
                authService.AdminId,
                authService.AdminName
            }.ToJson());
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
        [HttpGet("/api/[controller]/Permission/Exists")]
        public async Task<Result<bool>> GetPermissionExistsAsync([Required] long adminId, [Required] string onlyCode, CancellationToken cancellationToken = default)
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
        [HttpGet("/api/[controller]/Permission/Menu")]
        public async Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionMenusAsync([Required] long adminId, CancellationToken cancellationToken = default)
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
        [HttpGet("/api/[controller]/Permission/Express")]
        public async Task<Result<IList<PermissionMenuDto>>> GetPermissionMenuExpressAsync([Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.GetMenuExpressAsync(adminId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Record/List")]
        public async Task<Result<IList<LoginRecordDto>>> GetRecordListAsync([Required] int limit, [Required] long adminId, CancellationToken cancellationToken = default)
        {
            var res = await loginRecordService.GetListByAdminIdAsync(limit, adminId, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("/api/[controller]/Record/Page")]
        public async Task<Result<PagedModel<LoginRecordDto>>> GetRecordPageAsync([Required][FromQuery] LoginRecordQueryPagedCommand request, CancellationToken cancellationToken = default)
        {
            request.IsVaild();

            var res = await loginRecordService.GetPageListAsync(request, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
