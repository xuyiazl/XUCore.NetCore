using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence.Entities.Admin;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public interface IAdminMenuAppService : ICurdAppService<long, AdminMenuEntity, AdminMenuDto, AdminMenuCreateCommand, AdminMenuUpdateCommand, AdminMenuQueryCommand, AdminMenuQueryPagedCommand>, 
        IAppService
    {
        /// <summary>
        /// 更新导航指定字段内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateFieldAsync(long id, string field, string value, CancellationToken cancellationToken = default);
        /// <summary>
        /// 获取导航树形结构
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<AdminMenuTreeDto>>> GetListByTreeAsync(CancellationToken cancellationToken = default);
    }
}
