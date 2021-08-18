﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;
using XUCore.Template.EasyLayer.Core.Enums;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.AdminUser
{
    public interface IAdminUserService : IDbService
    {
        Task<bool> AnyByAccountAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken);
        Task<int> CreateAsync(AdminUserCreateCommand request, CancellationToken cancellationToken);
        Task<int> DeleteAsync(long[] ids, CancellationToken cancellationToken);
        Task<AdminUserDto> GetByAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken);
        Task<AdminUserDto> GetByIdAsync(long id, CancellationToken cancellationToken);
        Task<IList<long>> GetRoleKeysAsync(long adminId, CancellationToken cancellationToken);
        Task<PagedModel<AdminUserDto>> GetPagedListAsync(AdminUserQueryPagedCommand request, CancellationToken cancellationToken);
        Task<AdminUserDto> LoginAsync(AdminUserLoginCommand request, CancellationToken cancellationToken);
        Task<int> RelevanceRoleAsync(AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(AdminUserUpdateInfoCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, Status status, CancellationToken cancellationToken);

        Task<IList<AdminUserLoginRecordDto>> GetRecordListAsync(AdminUserLoginRecordQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<AdminUserLoginRecordDto>> GetRecordPageListAsync(AdminUserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken);
    }
}