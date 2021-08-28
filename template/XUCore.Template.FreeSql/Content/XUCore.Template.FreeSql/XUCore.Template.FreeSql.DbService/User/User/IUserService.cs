using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.FreeSql.Curd;
using XUCore.Paging;
using XUCore.Template.FreeSql.Core.Enums;
using XUCore.Template.FreeSql.Persistence.Entities.User;

namespace XUCore.Template.FreeSql.DbService.User.User
{
    public interface IUserService : ICurdService<long, UserEntity, UserDto, UserCreateCommand, UserUpdateInfoCommand, UserQueryCommand, UserQueryPagedCommand>, IDbService
    {
        Task<bool> AnyByAccountAsync(AccountMode accountMode, string account, long notId, CancellationToken cancellationToken);
        Task<UserDto> GetByAccountAsync(AccountMode accountMode, string account, CancellationToken cancellationToken);
        Task<IList<long>> GetRoleKeysAsync(long userId, CancellationToken cancellationToken);
        Task<UserDto> LoginAsync(UserLoginCommand request, CancellationToken cancellationToken);
        Task<int> CreateRelevanceRoleAsync(UserRelevanceRoleCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(UserUpdatePasswordCommand request, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, bool enabled, CancellationToken cancellationToken);

        Task<IList<UserLoginRecordDto>> GetRecordListAsync(UserLoginRecordQueryCommand request, CancellationToken cancellationToken);

        Task<PagedModel<UserLoginRecordDto>> GetRecordPagedListAsync(UserLoginRecordQueryPagedCommand request, CancellationToken cancellationToken);
    }
}