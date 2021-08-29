using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.EasyLayer.Persistence.Entities.Admin;

namespace XUCore.Template.EasyLayer.DbService.Admin.AdminRole
{
    public interface IAdminRoleService : ICurdService<long, AdminRoleEntity, AdminRoleDto, AdminRoleCreateCommand, AdminRoleUpdateCommand, AdminRoleQueryCommand, AdminRoleQueryPagedCommand>
    {
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken);
    }
}