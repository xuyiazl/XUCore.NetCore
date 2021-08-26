using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence.Entities;

namespace XUCore.Template.Easy.Applaction
{
    public interface ICurdAppService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand> : IAppService
            where TKey : struct
            where TDto : class, new()
            where TEntity : BaseEntity<TKey>, new()
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
        Task<Result<int>> CreateAsync(TCreateCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateAsync(TUpdateCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> UpdateStatusAsync(TKey[] ids, Status status, CancellationToken cancellationToken);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<int>> DeleteAsync(TKey[] ids, CancellationToken cancellationToken);
        /// <summary>
        /// 根据id获取一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<TDto>> GetByIdAsync(TKey id, CancellationToken cancellationToken);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<IList<TDto>>> GetListAsync(TListCommand request, CancellationToken cancellationToken);
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Result<PagedModel<TDto>>> GetPagedListAsync(TPageCommand request, CancellationToken cancellationToken);
    }
}
