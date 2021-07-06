using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;
using Sample.Plain.Core.Enums;

namespace Sample.Plain.DbService.Sys.Admin.AdminRole
{
    public interface IAdminRoleService : IDbService
    {
        Task<int> CreateAsync(AdminRoleCreateCommand request, CancellationToken cancellationToken);
        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);
        Task<IList<AdminRoleDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<AdminRoleDto> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<IList<long>> GetRelevanceMenuIdsAsync(int roleId, CancellationToken cancellationToken);
        Task<PagedModel<AdminRoleDto>> GetPageListAsync(AdminRoleQueryPagedCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(AdminRoleUpdateCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);
    }
}