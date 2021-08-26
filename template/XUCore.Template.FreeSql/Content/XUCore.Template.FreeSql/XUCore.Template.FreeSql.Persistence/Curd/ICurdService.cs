using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.Data;
using XUCore.Paging;
using XUCore.Template.FreeSql.Persistence.Entities;

namespace XUCore.Template.FreeSql.Persistence
{
    public interface ICurdService<TEntity, TKey, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>
            where TEntity : EntityFull<TKey>, new()
            where TKey : struct
            where TCreateCommand : CreateCommand
            where TUpdateCommand : UpdateCommand<TKey>
            where TListCommand : ListCommand
            where TPageCommand : PageCommand
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TKey> CreateAsync(TCreateCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(TUpdateCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(TKey[] ids, CancellationToken cancellationToken);
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SoftDeleteAsync(TKey id, CancellationToken cancellationToken);
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SoftDeleteAsync(TKey[] ids, CancellationToken cancellationToken);
        /// <summary>
        /// 根据id获取一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TDto> GetByIdAsync(TKey id, CancellationToken cancellationToken);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TDto>> GetListAsync(TListCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PagedModel<TDto>> GetPagedListAsync(TPageCommand request, CancellationToken cancellationToken);
    }
}