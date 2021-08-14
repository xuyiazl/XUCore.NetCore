using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.WebApi2.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.WebApi2.Template.DbService.Sys.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}