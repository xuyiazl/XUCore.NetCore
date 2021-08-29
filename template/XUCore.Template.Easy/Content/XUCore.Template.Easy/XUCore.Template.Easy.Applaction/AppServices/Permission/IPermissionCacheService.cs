using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Easy.Persistence.Entities.Admin;

namespace XUCore.Template.Easy.Applaction.Permission
{
    public interface IPermissionCacheService : IAppService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}