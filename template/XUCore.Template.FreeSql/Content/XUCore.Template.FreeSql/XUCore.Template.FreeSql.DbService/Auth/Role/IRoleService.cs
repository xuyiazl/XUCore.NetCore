using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.FreeSql.Persistence;
using XUCore.Template.FreeSql.Persistence.Entities.Sys.User;

namespace XUCore.Template.FreeSql.DbService.Auth.Role
{
    public interface IRoleService : ICurdService<RoleEntity, long, RoleDto, RoleCreateCommand, RoleUpdateCommand, RoleQueryCommand, RoleQueryPagedCommand>, IDbService
    {
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, bool enabled, CancellationToken cancellationToken);
        Task<IList<long>> GetRelevanceMenuAsync(int roleId, CancellationToken cancellationToken);
    }
}