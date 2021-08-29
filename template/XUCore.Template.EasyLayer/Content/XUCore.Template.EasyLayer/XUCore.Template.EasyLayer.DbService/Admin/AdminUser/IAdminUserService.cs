using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;
using XUCore.Template.EasyLayer.Core.Enums;
using XUCore.Template.EasyLayer.Persistence.Entities.Admin;

namespace XUCore.Template.EasyLayer.DbService.Admin.AdminUser
{
    public interface IAdminUserService : ICurdService<long, AdminUserEntity, AdminUserDto, AdminUserCreateCommand, AdminUserUpdateInfoCommand, AdminUserQueryCommand, AdminUserQueryPagedCommand>
    {
        Task<bool> AnyByAccountAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken);
        Task<AdminUserDto> GetByAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken);
        Task<IList<long>> GetRoleKeysAsync(long adminId, CancellationToken cancellationToken);
        Task<AdminUserDto> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken);
        Task<int> CreateRelevanceRoleAsync(AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);

        Task<IList<AdminUserLoginRecordDto>> GetRecordListAsync(AdminUserLoginRecordQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<AdminUserLoginRecordDto>> GetRecordPagedListAsync(AdminUserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken);
    }
}