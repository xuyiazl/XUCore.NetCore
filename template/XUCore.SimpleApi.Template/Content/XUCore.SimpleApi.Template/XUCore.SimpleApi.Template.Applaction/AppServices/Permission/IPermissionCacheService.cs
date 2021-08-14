using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.SimpleApi.Template.Applaction.Permission
{
    public interface IPermissionCacheService : IAppService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}