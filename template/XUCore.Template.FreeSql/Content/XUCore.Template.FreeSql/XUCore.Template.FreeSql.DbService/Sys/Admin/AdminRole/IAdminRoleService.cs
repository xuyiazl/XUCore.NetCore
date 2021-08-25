using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.FreeSql.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.FreeSql.DbService.Sys.Admin.AdminRole
{
    public interface IAdminRoleService : ICurdService<long, AdminRoleEntity, AdminRoleDto, AdminRoleCreateCommand, AdminRoleUpdateCommand, AdminRoleQueryCommand, AdminRoleQueryPagedCommand>
    {
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken);
    }
}