using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.SimpleApi.Template.Applaction.Permission
{
    public interface IPermissionService : IAppService
    {
        Task<bool> ExistsAsync(long adminId, string onlyCode, CancellationToken cancellationToken);
        Task<IList<PermissionMenuDto>> GetMenuExpressAsync(long adminId, CancellationToken cancellationToken);
        Task<IList<PermissionMenuTreeDto>> GetMenusAsync(long adminId, CancellationToken cancellationToken);
    }
}