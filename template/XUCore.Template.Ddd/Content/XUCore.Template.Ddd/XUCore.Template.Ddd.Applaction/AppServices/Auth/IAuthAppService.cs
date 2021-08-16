using XUCore.Template.Ddd.Applaction.Common.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Ddd.Domain.Auth.Menu;
using XUCore.Template.Ddd.Domain.Auth.Role;
using XUCore.Template.Ddd.Domain.User.User;
using XUCore.NetCore;
using XUCore.Paging;

namespace XUCore.Template.Ddd.Applaction.AppServices.User
{
    /// <summary>
    /// 权限管理
    /// </summary>
    public interface IAuthAppService : IAppService
    {
        #region [ 账号&角色 关联操作 ]

        /// <summary>
        /// 账号关联角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateUserAsync(UserRelevanceRoleCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取账号关联的角色id集合
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<string>>> GetUserAsync(UserQueryRoleKeys command, CancellationToken cancellationToken = default);

        #endregion

        #region [ 角色管理 ]

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateRoleAsync(RoleCreateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateRoleAsync(RoleUpdateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色指定字段内容
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateRoleAsync(RoleUpdateFieldCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新角色状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateRoleAsync(RoleUpdateStatusCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除角色（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteRoleAsync(RoleDeleteCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<RoleDto>> GetRoleAsync(RoleQueryDetail command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<RoleDto>>> GetRoleAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<RoleDto>>> GetRoleAsync(RoleQueryPaged command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<string>>> GetRoleAsync(RoleQueryMenuKeys command, CancellationToken cancellationToken = default);

        #endregion

        #region [ 权限导航操作 ]

        /// <summary>
        /// 创建导航
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> CreateMenuAsync(MenuCreateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateMenuAsync(MenuUpdateCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateMenuAsync(MenuUpdateFieldCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新导航状态
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateMenuAsync(MenuUpdateStatusCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 删除导航（物理删除）
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteMenuAsync(MenuDeleteCommand command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航信息
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<MenuDto>> GetMenuAsync(MenuQueryDetail command, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<MenuTreeDto>>> GetMenuAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航分页
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<MenuDto>>> GetMenuAsync(MenuQueryByWeight command, CancellationToken cancellationToken = default);

        #endregion
    }
}