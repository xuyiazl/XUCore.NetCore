using XUCore.Net5.Template.Applaction.Common.Interfaces;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Sys.AdminMenu;
using XUCore.Net5.Template.Domain.Sys.AdminRole;
using XUCore.Net5.Template.Domain.Sys.AdminUser;
using XUCore.Net5.Template.Domain.Sys.LoginRecord;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;

namespace XUCore.Net5.Template.Application.AppServices.Admin
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
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateUserAsync(AdminUserUpdateFieldCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateUserAsync(AdminUserUpdateStatusCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除账号（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteUserAsync(AdminUserDeleteCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AdminUserDto>> GetUserAsync(AdminUserQueryDetail command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号信息（根据账号或手机号码）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AdminUserDto>> GetUserAsync(AdminUserQueryByAccount command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 检查账号或者手机号是否存在
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<bool>> GetUserAsync(AdminUserAnyByAccount command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<AdminUserDto>>> GetUserAsync(AdminUserQueryPaged command, CancellationToken cancellationToken = default);

        #endregion

        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateUserAsync(AdminUserRelevanceRoleCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<long>>> GetUserAsync(AdminUserQueryRoleKeys command, CancellationToken cancellationToken = default);

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
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateRoleAsync(AdminRoleUpdateFieldCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateRoleAsync(AdminRoleUpdateStatusCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteRoleAsync(AdminRoleDeleteCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AdminRoleDto>> GetRoleAsync(AdminRoleQueryDetail command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<AdminRoleDto>>> GetRoleAsync(AdminRoleQueryAll command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<AdminRoleDto>>> GetRoleAsync(AdminRoleQueryPaged command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<long>>> GetRoleAsync(AdminRoleQueryMenuKeys command, CancellationToken cancellationToken = default);

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
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateMenuAsync(AdminMenuUpdateFieldCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateMenuAsync(AdminMenuUpdateStatusCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteMenuAsync(AdminMenuDeleteCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<AdminMenuDto>> GetMenuAsync(AdminMenuQueryDetail command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<AdminMenuTreeDto>>> GetMenuAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<AdminMenuDto>>> GetMenuAsync(AdminMenuQueryByWeight command, CancellationToken cancellationToken = default);

        #endregion
    }
}