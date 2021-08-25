using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.FreeSql.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.FreeSql.DbService.Sys.Admin.Permission
{
    public interface IPermissionCacheService : IDbService
    {
        Task<IList<AdminMenuEntity>> GetAllAsync(long adminId, CancellationToken cancellationToken);
    }
}