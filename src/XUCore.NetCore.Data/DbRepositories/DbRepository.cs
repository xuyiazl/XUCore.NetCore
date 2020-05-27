using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbRepositories
{
    public partial class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class
    {
        public DbContext Context { get; private set; }
        public DbSet<TEntity> Table { get; private set; }
        public DatabaseFacade Database { get; private set; }
        public bool IsNoTracking { get; set; }

        public DbRepository(DbContext dbContext)
        {
            Context = dbContext;

            Table = dbContext.Set<TEntity>();

            Database = dbContext.Database;

            IsNoTracking = true;
        }

        public IQueryable<TEntity> AsNoTracking()
        {
            if (IsNoTracking)
                return Table.AsNoTracking().AsQueryable();
            else
                return Table.AsQueryable();
        }

        public EntityEntry Entry(object entity)
        {
            return this.Context.Entry(entity);
        }

        public EntityEntry<TEntity> Entry(TEntity entity)
        {
            return this.Context.Entry<TEntity>(entity);
        }

        public bool Any(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = this.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);

            return query.Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = this.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);

            return await query.AnyAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);

            return await query.AnyAsync(cancellationToken);
        }

        public int Count(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = this.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);

            return query.Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var query = this.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync(cancellationToken);
        }
    }
}