using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sample.EasyLayer.Persistence.Entities.Sys.Admin;

namespace Sample.EasyLayer.DbService.Sys.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}