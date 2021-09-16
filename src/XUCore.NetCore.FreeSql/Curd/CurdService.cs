using AutoMapper;
using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.NetCore.FreeSql.Entity;
using XUCore.Paging;

namespace XUCore.NetCore.FreeSql.Curd
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
    public abstract class CurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand> : BaseRepository<TEntity, TKey>,
        ICurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>
           
            where TEntity : EntityFull<TKey>, new()
            where TDto : class, new()
            where TCreateCommand : CreateCommand
            where TUpdateCommand : UpdateCommand<TKey>
            where TListCommand : ListCommand
            where TPageCommand : PageCommand
    {
        protected readonly IFreeSql freeSql;
        protected readonly IBaseRepository<TEntity> repo;

        protected readonly IMapper mapper;
        /// <summary>
        /// 用户信息
        /// </summary>
        public IUser User { get; set; }

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
        /// <param name="freeSql"></param>
        /// <param name="mapper"></param>
        public CurdService(IFreeSql freeSql, IMapper mapper) : base(freeSql, null, null)
        {
            this.freeSql = freeSql;
            this.mapper = mapper;
            this.repo = freeSql.GetRepository<TEntity>();
        }
        /// <summary>
        /// CURD服务
        /// </summary>
        /// <param name="freeSql"></param>
        /// <param name="mapper"></param>
        /// <param name="filter"></param>
        /// <param name="asTable"></param>
        public CurdService(IFreeSql freeSql, IMapper mapper, Expression<Func<TEntity, bool>> filter, Func<string, string> asTable = null) : base(freeSql, filter, asTable)
        {
            this.freeSql = freeSql;
            this.mapper = mapper;
            this.repo = freeSql.GetRepository<TEntity>();
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

            var res = await repo.InsertAsync(entity, cancellationToken);

            if (res != null)
            {
                CreatedAction?.Invoke(res);

                return res.Id;
            }

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
            var entity = await repo.Select.WhereDynamic(request.Id).ToOneAsync<TEntity>(cancellationToken);

            if (entity == null)
                return 0;

            entity = mapper.Map(request, entity);

            var res = await repo.UpdateAsync(entity, cancellationToken);

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
            var res = await freeSql.Delete<TEntity>(ids).ExecuteAffrowsAsync(cancellationToken);

            if (res > 0)
                DeletedAction?.Invoke(ids);
            return res;
        }
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> SoftDeleteAsync(TKey id, CancellationToken cancellationToken)
        {
            var res = await repo.UpdateDiy
                 .SetDto(new TEntity
                 {
                     IsDeleted = true,
                     ModifiedAtUserId = User.Id.ToLong(),
                     ModifiedAtUserName = User.UserName
                 })
                 .WhereDynamic(id)
                 .ExecuteAffrowsAsync(cancellationToken);
            return res;
        }
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<int> SoftDeleteAsync(TKey[] ids, CancellationToken cancellationToken)
        {
            var res = await repo.UpdateDiy
                .SetDto(new TEntity
                {
                    IsDeleted = true,
                    ModifiedAtUserId = User.Id.ToLong(),
                    ModifiedAtUserName = User.UserName
                })
                .WhereDynamic(ids)
                .ExecuteAffrowsAsync(cancellationToken);
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
            return await repo.Select.WhereDynamic(id).ToOneAsync<TDto>(cancellationToken);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<IList<TDto>> GetListAsync(TListCommand request, CancellationToken cancellationToken)
        {
            var select = repo.Select.OrderBy(c => c.Id);

            if (request.Limit > 0)
                select = select.Take(request.Limit);

            var res = await select.ToListAsync<TDto>(cancellationToken);

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
            var res = await repo.Select.OrderBy(c => c.Id).ToPagedListAsync<TEntity, TDto>(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
