using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sample.Layer.Core.Enums;
using Sample.Layer.Persistence.Entities.Sys.Admin;

namespace Sample.Layer.DbService.Sys.Admin.AdminMenu
{
    public interface IAdminMenuService : ICurdService<long, AdminMenuEntity, AdminMenuDto, AdminMenuCreateCommand, AdminMenuUpdateCommand, AdminMenuQueryCommand, AdminMenuQueryPagedCommand>
    {
        Task<IList<AdminMenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
    }
}