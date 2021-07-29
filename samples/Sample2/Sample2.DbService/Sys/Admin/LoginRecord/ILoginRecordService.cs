using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;

namespace Sample2.DbService.Sys.Admin.LoginRecord
{
    public interface ILoginRecordService : IDbService
    {
        Task<int> CreateAsync(LoginRecordCreateCommand request, CancellationToken cancellationToken);
        Task<IList<LoginRecordDto>> GetListByAdminIdAsync(int limit, long adminId, CancellationToken cancellationToken);
        Task<PagedModel<LoginRecordDto>> GetPageListAsync(LoginRecordQueryPagedCommand request, CancellationToken cancellationToken);
    }
}