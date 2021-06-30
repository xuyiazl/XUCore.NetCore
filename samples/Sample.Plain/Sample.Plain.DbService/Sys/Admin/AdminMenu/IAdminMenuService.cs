using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sample.Plain.Persistence.Entities.Enums;

namespace Sample.Plain.DbService.Sys.Admin.AdminMenu
{
    public interface IAdminMenuService : IDbService
    {
        Task<int> CreateAsync(AdminMenuCreateCommand request, CancellationToken cancellationToken);
        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);
        Task<AdminMenuDto> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<IList<AdminMenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
        Task<IList<AdminMenuDto>> GetListByWeightAsync(bool isMenu, CancellationToken cancellationToken);
        Task<int> UpdateAsync(AdminMenuUpdateCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);
    }
}