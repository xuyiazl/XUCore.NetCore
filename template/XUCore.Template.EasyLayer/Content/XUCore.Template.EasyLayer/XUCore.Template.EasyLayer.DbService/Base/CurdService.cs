using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.NetCore.Data;
using XUCore.Template.EasyLayer.Core.Enums;
using XUCore.Template.EasyLayer.Persistence.Entities;

namespace XUCore.Template.EasyLayer.DbService
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
    public abstract class CurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand> :
        CurdServiceProvider<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>,
        ICurdService<TKey, TEntity, TDto, TCreateCommand, TUpdateCommand, TListCommand, TPageCommand>, IDbService
            where TKey : struct
            where TDto : class, new()
            where TEntity : BaseEntity<TKey>, new()
            where TCreateCommand : CreateCommand
            where TUpdateCommand : UpdateCommand<TKey>
            where TListCommand : ListCommand
            where TPageCommand : PageCommand
    {
        /// <summary>
        /// CURD服务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="mapper"></param>
        public CurdService(IDbRepository<TEntity> db, IMapper mapper) : base(db, mapper)
        {
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
                    return await db.UpdateAsync(c => ids.Contains(c.Id), c => new TEntity { Status = Status.Show, UpdatedAt = DateTime.Now }, cancellationToken);
                case Status.SoldOut:
                    return await db.UpdateAsync(c => ids.Contains(c.Id), c => new TEntity { Status = Status.SoldOut, UpdatedAt = DateTime.Now }, cancellationToken);
                case Status.Trash:
                    return await db.UpdateAsync(c => ids.Contains(c.Id), c => new TEntity { Status = Status.Trash, DeletedAt = DateTime.Now }, cancellationToken);
                default:
                    return 0;
            }
        }
    }
}
