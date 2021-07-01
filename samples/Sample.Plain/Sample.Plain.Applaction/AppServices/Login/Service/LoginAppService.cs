﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Serializer;
using Sample.Plain.Applaction.Authorization;
using Sample.Plain.Core;
using Sample.Plain.DbService.Sys.Admin.AdminUser;
using Sample.Plain.DbService.Sys.Admin.LoginRecord;
using Sample.Plain.DbService.Sys.Admin.Permission;

namespace Sample.Plain.Applaction.Login
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
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
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<LoginTokenDto>> LoginAsync(AdminUserLoginCommand command, CancellationToken cancellationToken)
        {
            (var accessToken, var refreshToken) = await authService.LoginAsync(command, cancellationToken);

            return Success(SubCode.Success, new LoginTokenDto
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
            return Success(SubCode.Success, data: new
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
        public async Task<Result<bool>> GetPermissionExistsAsync(long adminId, string onlyCode, CancellationToken cancellationToken = default)
        {
            var res = await permissionService.ExistsAsync(adminId, onlyCode, cancellationToken);

            return Success(SubCode.Success, res);
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

            return Success(SubCode.Success, res);
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

            return Success(SubCode.Success, res);
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
        public async Task<Result<IList<LoginRecordDto>>> GetRecordListAsync(int limit, long adminId, CancellationToken cancellationToken = default)
        {
            var res = await loginRecordService.GetListByAdminIdAsync(limit, adminId, cancellationToken);

            return Success(SubCode.Success, res);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<LoginRecordDto>>> GetRecordPageAsync(LoginRecordQueryPagedCommand command, CancellationToken cancellationToken = default)
        {
            var res = await loginRecordService.GetPageListAsync(command, cancellationToken);

            return Success(SubCode.Success, res);
        }

        #endregion
    }
}