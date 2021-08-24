using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sample.EasyLayer.Core.Enums;
using Sample.EasyLayer.Persistence.Entities.Sys.Admin;

namespace Sample.EasyLayer.DbService.Sys.Admin.AdminMenu
{
    public interface IAdminMenuService : ICurdService<long, AdminMenuEntity, AdminMenuDto, AdminMenuCreateCommand, AdminMenuUpdateCommand, AdminMenuQueryCommand, AdminMenuQueryPagedCommand>
    {
        Task<IList<AdminMenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
    }
}