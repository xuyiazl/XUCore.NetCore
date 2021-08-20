using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Extensions;
using XUCore.Paging;
using XUCore.Template.Layer.Core.Enums;
using XUCore.Template.Layer.Persistence;
using XUCore.Template.Layer.Persistence.Entities;

namespace XUCore.Template.Layer.DbService
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
    public abstract class CurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>
        : ICurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>, IDbService
            where TDto : class, new()
            where TEntity : BaseEntity<TKey>, new()
            where TCreateCommand : CreateCommand
            where TUpdateCommand : UpdateCommand<TKey>
            where TListCommand : ListCommand
            where TPageCommand : PageCommand
    {
        protected readonly IDefaultDbRepository db;
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
        public CurdService(IDefaultDbRepository db, IMapper mapper)
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
        public virtual async Task<int> CreateAsync(TCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<TCreateCommand, TEntity>(request);

            var res = await db.AddAsync(entity, cancellationToken: cancellationToken);

            if (res > 0)
                CreatedAction?.Invoke(entity);

            return res;
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateAsync(TUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await db.GetFirstAsync<TEntity>(c => c.Id.Equals(request.Id), cancellationToken: cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = db.Update(entity);

            if (res > 0)
                UpdatedAction?.Invoke(entity);

            return res;
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(TKey[] ids, Status status, CancellationToken cancellationToken)
        {
            switch (status)
            {
                case Status.Show:
                    return await db.UpdateAsync<TEntity>(c => ids.Contains(c.Id), c => new TEntity { Status = Status.Show, UpdatedAt = DateTime.Now }, cancellationToken);
                case Status.SoldOut:
                    return await db.UpdateAsync<TEntity>(c => ids.Contains(c.Id), c => new TEntity { Status = Status.SoldOut, UpdatedAt = DateTime.Now }, cancellationToken);
                case Status.Trash:
                    return await db.UpdateAsync<TEntity>(c => ids.Contains(c.Id), c => new TEntity { Status = Status.Trash, DeletedAt = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> DeleteAsync(TKey[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync<TEntity>(c => ids.Contains(c.Id), cancellationToken);

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
            var res = await db.GetByIdAsync<TEntity, TDto>(id, cancellationToken);

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
            var selector = db.AsQuery<TEntity>();

            var res = await db.GetListAsync<TEntity, TDto>(selector: selector, orderby: request.Orderby, skip: -1, limit: request.Limit, cancellationToken);

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
            var selector = db.AsQuery<TEntity>();

            var res = await db.GetPagedListAsync<TEntity, TDto>(selector: selector, orderby: request.Orderby, currentPage: request.CurrentPage, pageSize: request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
