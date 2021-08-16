﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using Sample.Layer.Core.Enums;
using Sample.Layer.DbService.Sys.Admin.AdminMenu;
using Sample.Layer.DbService.Sys.Admin.AdminRole;
using Sample.Layer.DbService.Sys.Admin.AdminUser;

namespace Sample.Layer.Applaction.Admin
{
    /// <summary>
    /// 管理员管理
    /// </summary>
    public interface IAdminAppService : IAppService
    {

        #region [ 账号管理 ]

        /// <summary>
        /// 创建管理员账号
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateUserAsync(AdminUserCreateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateUserAsync(AdminUserUpdateInfoCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateUserAsync(AdminUserUpdatePasswordCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateUserAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateUserAsync(long[] ids, Status status, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteUserAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AdminUserDto>> GetUserAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AdminUserDto>> GetUserByAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken = default);
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="notId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<bool>> GetUserAnyAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<AdminUserDto>>> GetUserPagedAsync(AdminUserQueryPagedCommand command, CancellationToken cancellationToken = default);

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateUserRelevanceRoleIdAsync(AdminUserRelevanceRoleCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<long>>> GetUserRelevanceRoleIdsAsync(long adminId, CancellationToken cancellationToken = default);

        #endregion

        #region [ 角色管理 ]

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateRoleAsync(AdminRoleCreateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateRoleAsync(AdminRoleUpdateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateRoleAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateRoleAsync(long[] ids, Status status, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteRoleAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AdminRoleDto>> GetRoleAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<AdminRoleDto>>> GetRoleAllAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<AdminRoleDto>>> GetRolePagedAsync(AdminRoleQueryPagedCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<long>>> GetRoleRelevanceMenuIdsAsync(int roleId, CancellationToken cancellationToken = default);

        #endregion

        #region [ 权限导航操作 ]

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateMenuAsync(AdminMenuCreateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateMenuAsync(AdminMenuUpdateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateMenuAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateMenuAsync(long[] ids, Status status, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteMenuAsync(long[] ids, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AdminMenuDto>> GetMenuAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<AdminMenuTreeDto>>> GetMenuByTreeAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="isMenu"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<AdminMenuDto>>> GetMenuByWeightAsync(bool isMenu = true, CancellationToken cancellationToken = default);

        #endregion
    }
}