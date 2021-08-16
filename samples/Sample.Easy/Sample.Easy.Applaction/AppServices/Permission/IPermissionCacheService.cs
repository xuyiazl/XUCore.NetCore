using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sample.Easy.Persistence.Entities.Sys.Admin;

namespace Sample.Easy.Applaction.Permission
{
    public interface IPermissionCacheService : IAppService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}