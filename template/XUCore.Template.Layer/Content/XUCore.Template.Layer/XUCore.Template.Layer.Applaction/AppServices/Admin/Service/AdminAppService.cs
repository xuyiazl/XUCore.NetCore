using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.Layer.Core;
using XUCore.Template.Layer.Core.Enums;
using XUCore.Template.Layer.DbService.Admin.AdminMenu;
using XUCore.Template.Layer.DbService.Admin.AdminRole;
using XUCore.Template.Layer.DbService.Admin.AdminUser;

namespace XUCore.Template.Layer.Applaction.Admin
{
    /// <summary>
    /// 管理员管理
    /// </summary>
    public class AdminAppService : AppService, IAdminAppService
    {
        private readonly IAdminMenuService adminMenuService;
        private readonly IAdminRoleService adminRoleService;
        private readonly IAdminUserService adminUserService;

        public AdminAppService(IServiceProvider serviceProvider)
        {
            this.adminMenuService = serviceProvider.GetService<IAdminMenuService>();
            this.adminRoleService = serviceProvider.GetService<IAdminRoleService>();
            this.adminUserService = serviceProvider.GetService<IAdminUserService>();
        }

        #region [ 账号管理 ]

        /// <summary>
        /// 创建管理员账号
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<long>> CreateUserAsync(AdminUserCreateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.CreateAsync(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新账号信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateUserAsync(AdminUserUpdateInfoCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateUserAsync(AdminUserUpdatePasswordCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateUserAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(id, field, value, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateUserAsync(long[] ids, Status status, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.UpdateAsync(ids, status, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> DeleteUserAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<AdminUserDto>> GetUserAsync(long id, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<AdminUserDto>> GetUserByAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetByAccountAsync(accountMode, account, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="accountMode"></param>
        /// <param name="account"></param>
        /// <param name="notId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<bool>> GetUserAnyAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.AnyByAccountAsync(accountMode, account, notId, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<AdminUserDto>>> GetUserPagedAsync(AdminUserQueryPagedCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetPagedListAsync(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> CreateUserRelevanceRoleIdAsync(AdminUserRelevanceRoleCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.CreateRelevanceRoleAsync(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<long>>> GetUserRelevanceRoleIdsAsync(long adminId, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetRoleKeysAsync(adminId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 登录记录 ]

        /// <summary>
        /// 获取最近登录记录
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<AdminUserLoginRecordDto>>> GetRecordListAsync(AdminUserLoginRecordQueryCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetRecordListAsync(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有登录记录分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<AdminUserLoginRecordDto>>> GetRecordPageAsync(AdminUserLoginRecordQueryPagedCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminUserService.GetRecordPagedListAsync(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 角色管理 ]

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<long>> CreateRoleAsync(AdminRoleCreateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.CreateAsync(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateRoleAsync(AdminRoleUpdateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.UpdateAsync(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateRoleAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.UpdateAsync(id, field, value, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateRoleAsync(long[] ids, Status status, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.UpdateAsync(ids, status, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> DeleteRoleAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<AdminRoleDto>> GetRoleAsync(long id, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<AdminRoleDto>>> GetRoleListAsync(AdminRoleQueryCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.GetListAsync(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<PagedModel<AdminRoleDto>>> GetRolePagedAsync(AdminRoleQueryPagedCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.GetPagedListAsync(command, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<long>>> GetRoleRelevanceMenuAsync(int roleId, CancellationToken cancellationToken = default)
        {
            var res = await adminRoleService.GetRelevanceMenuAsync(roleId, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion

        #region [ 权限导航操作 ]

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<long>> CreateMenuAsync(AdminMenuCreateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.CreateAsync(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateMenuAsync(AdminMenuUpdateCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.UpdateAsync(command, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateMenuAsync(long id, string field, string value, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.UpdateAsync(id, field, value, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新导航状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateMenuAsync(long[] ids, Status status, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.UpdateAsync(ids, status, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> DeleteMenuAsync(long[] ids, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.DeleteAsync(ids, cancellationToken);

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<AdminMenuDto>> GetMenuAsync(long id, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.GetByIdAsync(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<AdminMenuTreeDto>>> GetMenuByTreeAsync(CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.GetListByTreeAsync(cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取导航列表
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<IList<AdminMenuDto>>> GetMenuListAsync(AdminMenuQueryCommand command, CancellationToken cancellationToken = default)
        {
            var res = await adminMenuService.GetListAsync(command, cancellationToken);

            return RestFull.Success(data: res);
        }

        #endregion
    }
}
