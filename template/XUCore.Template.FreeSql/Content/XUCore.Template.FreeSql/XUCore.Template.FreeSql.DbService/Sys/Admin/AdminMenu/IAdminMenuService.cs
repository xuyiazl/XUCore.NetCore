using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.FreeSql.Core.Enums;
using XUCore.Template.FreeSql.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.FreeSql.DbService.Sys.Admin.AdminMenu
{
    public interface IAdminMenuService : ICurdService<long, AdminMenuEntity, AdminMenuDto, AdminMenuCreateCommand, AdminMenuUpdateCommand, AdminMenuQueryCommand, AdminMenuQueryPagedCommand>
    {
        Task<IList<AdminMenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
    }
}