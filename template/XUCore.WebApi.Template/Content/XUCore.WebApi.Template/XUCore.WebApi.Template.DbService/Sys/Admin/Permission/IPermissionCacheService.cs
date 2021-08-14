using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.WebApi.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.WebApi.Template.DbService.Sys.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}