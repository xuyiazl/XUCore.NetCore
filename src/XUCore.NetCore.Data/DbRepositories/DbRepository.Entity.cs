using Microsoft.EntityFrameworkCore;
using XUCore.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbRepositories
{
    public partial class DbRepository<TEntity> : IDbRepository<TEntity> where TEntity : class
    {
        public TEntity GetEntity(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                return query.FirstOrDefault(selector);
            else
                return query.FirstOrDefault();
        }

        public TEntity GetEntity<TOrder>(
            Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return query.FirstOrDefault();
        }

        public TEntity GetEntity(
            Expression<Func<TEntity, bool>> selector = null,
            string orderBy = "")
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return query.FirstOrDefault();
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                return await query.FirstOrDefaultAsync(selector);
            else
                return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetEntityAsync<TOrder>(Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null,
            string orderBy = "")
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                return await query.FirstOrDefaultAsync(selector, cancellationToken);
            else
                return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity> GetEntityAsync<TOrder>(Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            if (selector != null)
                query = query.Where(selector);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> selector = null,
            string orderBy = "",
            CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            if (selector != null)
                query = query.Where(selector);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public TResult GetEntity<TResult>(
            Expression<Func<TEntity, TResult>> converter,
            Expression<Func<TEntity, bool>> selector = null)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            return query
                .Select(converter)
                .FirstOrDefault();
        }

        public TResult GetEntity<TResult, TOrder>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return query
                .Select(converter)
                .FirstOrDefault();
        }

        public TResult GetEntity<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null,
            string orderBy = "")
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return query
                .Select(converter)
                .FirstOrDefault();
        }

        public async Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync();
        }

        public async Task<TResult> GetEntityAsync<TResult, TOrder>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync();
        }

        public async Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null,
            string orderBy = "")
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync();
        }

        public async Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null, CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TResult> GetEntityAsync<TResult, TOrder>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null,
            Expression<Func<TEntity, TOrder>> orderBy = null,
            bool orderDesc = false,
            CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (orderBy != null)
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TResult> GetEntityAsync<TResult>(Expression<Func<TEntity, TResult>> converter, Expression<Func<TEntity, bool>> selector = null,
            string orderBy = "",
            CancellationToken cancellationToken = default)
        {
            var query = this.AsNoTracking();
            if (selector != null)
                query = query.Where(selector);
            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderByBatch(orderBy);
            return await query
                .Select(converter)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}