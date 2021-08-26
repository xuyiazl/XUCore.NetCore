using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.NetCore;
using XUCore.Paging;
using XUCore.Template.Easy.Core;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence;
using XUCore.Template.Easy.Persistence.Entities;

namespace XUCore.Template.Easy.Applaction
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
    public abstract class CurdAppService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>
        : AppService, ICurdAppService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>
            where TKey : struct
            where TDto : class, new()
            where TEntity : BaseEntity<TKey>, new()
            where TCreateCommand : CreateCommand
            where TUpdateCommand : UpdateCommand<TKey>
            where TListCommand : ListCommand
            where TPageCommand : PageCommand
    {
        protected readonly IDefaultDbRepository<TEntity> db;
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
        public CurdAppService(IDefaultDbRepository<TEntity> db, IMapper mapper)
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
        public virtual async Task<Result<int>> CreateAsync([Required][FromBody] TCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<TCreateCommand, TEntity>(request);

            var res = await db.AddAsync(entity, cancellationToken: cancellationToken);

            if (res > 0)
            {
                CreatedAction?.Invoke(entity);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Result<int>> UpdateAsync([Required][FromBody] TUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await db.GetByIdAsync<TEntity>(request.Id, cancellationToken);

            if (entity == null)
                return RestFull.Fail(data: 0);

            entity = mapper.Map(request, entity);

            var res = db.Update(entity);

            if (res > 0)
            {
                UpdatedAction?.Invoke(entity);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> UpdateStatusAsync([Required][FromQuery] TKey[] ids, [Required][FromQuery] Status status, CancellationToken cancellationToken)
        {
            var res = 0;

            switch (status)
            {
                case Status.Show:
                    res = await db.UpdateAsync(c => ids.Contains(c.Id), c => new TEntity { Status = Status.Show, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case Status.SoldOut:
                    res = await db.UpdateAsync(c => ids.Contains(c.Id), c => new TEntity { Status = Status.SoldOut, UpdatedAt = DateTime.Now }, cancellationToken);
                    break;
                case Status.Trash:
                    res = await db.UpdateAsync(c => ids.Contains(c.Id), c => new TEntity { Status = Status.Trash, DeletedAt = DateTime.Now }, cancellationToken);
                    break;
                default:
                    res = 0;
                    break;
            }

            if (res > 0)
                return RestFull.Success(data: res);
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Result<int>> DeleteAsync([Required] TKey[] ids, CancellationToken cancellationToken)
        {
            var res = await db.DeleteAsync(c => ids.Contains(c.Id), cancellationToken);

            if (res > 0)
            {
                DeletedAction?.Invoke(ids);

                return RestFull.Success(data: res);
            }
            else
                return RestFull.Fail(data: res);
        }
        /// <summary>
        /// 根据id获取一条记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [Route("/api/[controller]/{id}")]
        public virtual async Task<Result<TDto>> GetByIdAsync([Required] TKey id, CancellationToken cancellationToken)
        {
            var res = await db.GetByIdAsync<TDto>(id, cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Result<IList<TDto>>> GetListAsync([Required][FromQuery] TListCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter();

            var res = await db.GetListAsync<TDto>(selector: selector, orderby: $"{nameof(BaseEntity<TKey>.Id)} asc", skip: -1, limit: request.Limit, cancellationToken: cancellationToken);

            return RestFull.Success(data: res);
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<Result<PagedModel<TDto>>> GetPagedListAsync([Required][FromQuery] TPageCommand request, CancellationToken cancellationToken)
        {
            var selector = db.BuildFilter();

            var res = await db.GetPagedListAsync<TDto>(selector: selector, orderby: $"{nameof(BaseEntity<TKey>.Id)} asc", currentPage: request.CurrentPage, pageSize: request.PageSize, cancellationToken: cancellationToken);

            return RestFull.Success(data: res.ToModel());
        }
    }
}
