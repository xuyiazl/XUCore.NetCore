using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Template.Easy.Persistence.Entities.Admin;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public interface IAdminRoleAppService : ICurdAppService<long, AdminRoleEntity, AdminRoleDto, AdminRoleCreateCommand, AdminRoleUpdateCommand, AdminRoleQueryCommand, AdminRoleQueryPagedCommand>,
        IAppService
    {
        /// <summary>
        /// 更新角色指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取角色关联的所有导航id集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<long>>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken = default);
    }
}
