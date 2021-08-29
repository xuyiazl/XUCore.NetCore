using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.EasyLayer.Core.Enums;
using XUCore.Template.EasyLayer.Persistence.Entities.Admin;

namespace XUCore.Template.EasyLayer.DbService.Admin.AdminMenu
{
    public interface IAdminMenuService : ICurdService<long, AdminMenuEntity, AdminMenuDto, AdminMenuCreateCommand, AdminMenuUpdateCommand, AdminMenuQueryCommand, AdminMenuQueryPagedCommand>
    {
        Task<IList<AdminMenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
    }
}