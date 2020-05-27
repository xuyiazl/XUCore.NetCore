using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace XUCore.NetCore.Data.DbRepositories
{
    public interface IDbChangeRepository<TEntity> where TEntity : class
    {
        DbContext Context { get; }
        DbSet<TEntity> Table { get; }
        DatabaseFacade Database { get; }
        bool IsNoTracking { get; set; }

        void Add(TEntity entity);

        void AddAsync(TEntity entity);

        void AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void AddRange(IList<TEntity> Entities);

        void AddRangeAsync(IList<TEntity> Entities);

        void AddRangeAsync(IList<TEntity> Entities, CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        void Update(TEntity entity, params Expression<Func<TEntity, object>>[] updatedProperties);

        void UpdateRange(IList<TEntity> entities);

        void Delete(Expression<Func<TEntity, bool>> selector);

        void Delete(TEntity Entity);

        void DeleteRange(Expression<Func<TEntity, bool>> selector);

        void DeleteRange(IList<TEntity> entities);
    }
}