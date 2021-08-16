using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sample.Layer.Persistence.Entities.Sys.Admin;

namespace Sample.Layer.DbService.Sys.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}