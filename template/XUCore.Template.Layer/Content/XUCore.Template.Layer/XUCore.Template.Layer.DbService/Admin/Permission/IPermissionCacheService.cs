using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Layer.Persistence.Entities.Admin;

namespace XUCore.Template.Layer.DbService.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}