using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.FreeSql.Persistence;
using XUCore.Template.FreeSql.Persistence.Entities.Sys.User;

namespace XUCore.Template.FreeSql.DbService.Auth.Menu
{
    public interface IMenuService : ICurdService<MenuEntity, long, MenuDto, MenuCreateCommand, MenuUpdateCommand, MenuQueryCommand, MenuQueryPagedCommand>, IDbService
    {
        Task<IList<MenuTreeDto>> GetListByTreeAsync(CancellationToken cancellationToken);
        Task<int> UpdateAsync(long id, string field, string value, CancellationToken cancellationToken);
        Task<int> UpdateAsync(long[] ids, bool enabled, CancellationToken cancellationToken);
    }
}