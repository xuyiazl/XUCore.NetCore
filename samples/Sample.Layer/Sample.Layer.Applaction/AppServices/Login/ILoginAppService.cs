﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using Sample.Layer.DbService.Sys.Admin.AdminUser;
using Sample.Layer.DbService.Sys.Admin.LoginRecord;
using Sample.Layer.DbService.Sys.Admin.Permission;

namespace Sample.Layer.Applaction.Login
{
    /// <summary>
    /// 管理员登录接口
    /// </summary>
    public interface ILoginAppService : IAppService
    {

        #region [ 登录 ]

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<LoginTokenDto>> LoginAsync(AdminUserLoginCommand command, CancellationToken cancellationToken);
        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<string>> VerifyTokenAsync(CancellationToken cancellationToken);
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task LoginOutAsync(CancellationToken cancellationToken);

        #endregion

        #region [ 登录后的权限获取 ]

        /// <summary>
        /// 查询是否有权限
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="onlyCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<bool>> GetPermissionExistsAsync(long adminId, string onlyCode, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查询权限导航
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<PermissionMenuTreeDto>>> GetPermissionMenusAsync(long adminId, CancellationToken cancellationToken = default);
        /// <summary>
        /// 查询权限导航（快捷导航）
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<PermissionMenuDto>>> GetPermissionMenuExpressAsync(long adminId, CancellationToken cancellationToken = default);

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<LoginRecordDto>>> GetRecordListAsync(int limit, long adminId, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<LoginRecordDto>>> GetRecordPageAsync(LoginRecordQueryPagedCommand command, CancellationToken cancellationToken = default);

        #endregion
    }
}