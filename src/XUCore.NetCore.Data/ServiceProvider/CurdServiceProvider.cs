using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Ddd.Domain.Commands;
using XUCore.Paging;

namespace XUCore.NetCore.Data
{
    /// <summary>
    /// CURD服务
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    /// <typeparam name="TEntity">数据库实体</typeparam>
    /// <typeparam name="TDto">输出dto</typeparam>
    /// <typeparam name="TCreateCommand">创建命令</typeparam>
    /// <typeparam name="TUpdateCommand">修改命令</typeparam>
    /// <typeparam name="TListCommand">查询列表命令</typeparam>
    /// <typeparam name="TPageCommand">分页命令</typeparam>
    public abstract class CurdServiceProvider<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>
        : ICurdServiceProvider<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>
            where TKey : struct
            where TDto : class, new()
            where TEntity : Entity<TKey>, new()
            where TCreateCommand : CreateCommand
            where TUpdateCommand : UpdateCommand<TKey>
            where TListCommand : ListCommand
            where TPageCommand : PageCommand
    {
        protected readonly IDbRepository<TEntity> db;
        protected readonly IMapper mapper;
        /// <summary>
        /// 创建事件
        /// </summary>
        protected Action<TEntity> CreatedAction { get; set; }
        /// <summary>
        /// 修改事件
        /// </summary>
        protected Action<TEntity> UpdatedAction { get; set; }
        /// <summary>
        /// 删除事件
        /// </summary>
        protected Action<IList<TKey>> DeletedAction { get; set; }
        /// <summary>
        /// CURD服务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="mapper"></param>
        public CurdServiceProvider(IDbRepository<TEntity> db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TKey> CreateAsync(TCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<TCreateCommand, TEntity>(request);

            var res = await db.AddAsync(entity, cancellationToken: cancellationToken);

            if (res > 0)
            {
                CreatedAction?.Invoke(entity);

                return entity.Id;
            }
            else
                return default(TKey);
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await db.GetByIdAsync<TEntity>(request.Id, cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = db.Update(entity);

            if (res > 0)
                UpdatedAction?.Invoke(entity);

            return res;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TKey[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
                DeletedAction?.Invoke(ids);
            return res;
        }
        /// <summary>
        /// 根据id获取一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<TDto> GetByIdAsync(TKey id, CancellationToken cancellationToken)
        {
            var res = await db.GetByIdAsync<TDto>(id, cancellationToken);

            return res;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<IList<TDto>> GetListAsync(TListCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter();

            var res = await db.GetListAsync<TDto>(selector: selector, orderby: $"{nameof(Entity<TKey>.Id)} asc", skip: -1, limit: request.Limit, cancellationToken: cancellationToken);

            return res;
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<PagedModel<TDto>> GetPagedListAsync(TPageCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter();

            var res = await db.GetPagedListAsync<TDto>(selector: selector, orderby: $"{nameof(Entity<TKey>.Id)} asc", currentPage: request.CurrentPage, pageSize: request.PageSize, cancellationToken: cancellationToken);

            return res.ToModel();
        }
    }
}
