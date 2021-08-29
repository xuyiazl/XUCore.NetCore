using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.EasyLayer.Persistence.Entities.Admin;

namespace XUCore.Template.EasyLayer.DbService.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}