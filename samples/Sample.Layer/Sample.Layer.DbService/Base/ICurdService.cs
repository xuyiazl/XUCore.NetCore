using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.Data;
using Sample.Layer.Core.Enums;

namespace Sample.Layer.DbService
{
    public interface ICurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand> :
        ICurdServiceProvider<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>,
        IDbService
        where TCreateCommand : CreateCommand
        where TUpdateCommand : UpdateCommand<TKey>
        where TListCommand : ListCommand
        where TPageCommand : PageCommand
    {
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(TKey[] ids, Status status, CancellationToken cancellationToken);
    }
}