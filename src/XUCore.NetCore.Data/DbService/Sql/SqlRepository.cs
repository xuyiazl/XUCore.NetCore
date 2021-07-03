using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions.Datas;

namespace XUCore.NetCore.Data.DbService
{
    public class SqlRepository : ISqlRepository
    {
        private readonly IDbContext dbContext;
        public SqlRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual TEntity SqlFirstOrDefault<TEntity>(string sql, object model = null, CommandType type = CommandType.Text) where TEntity : class, new()
        {
            var res = SqlQuery<TEntity>(sql, model, type);

            return res.Count > 0 ? res[0] : default;
        }

        public virtual async Task<TEntity> SqlFirstOrDefaultAsync<TEntity>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var res = await SqlQueryAsync<TEntity>(sql, model, type, cancellationToken);

            return res.Count > 0 ? res[0] : default;
        }

        public virtual IList<TEntity> SqlQuery<TEntity>(string sql, object model = null, CommandType type = CommandType.Text) where TEntity : class, new()
        {
            return ExecuteReader(sql, model, type).ToList<TEntity>();
        }

        public virtual async Task<IList<TEntity>> SqlQueryAsync<TEntity>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default) where TEntity : class, new()
        {
            var res = await ExecuteReaderAsync(sql, model, type, cancellationToken);

            return res.ToList<TEntity>();
        }

        public virtual DataTable ExecuteReader(string sql, object model = null, CommandType type = CommandType.Text)
            => dbContext.Database.ExecuteReader(sql, model, type);

        public virtual async Task<DataTable> ExecuteReaderAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await dbContext.Database.ExecuteReaderAsync(sql, model, type, cancellationToken);

        public virtual DataSet DataAdapterFill(string sql, object model = null, CommandType type = CommandType.Text)
            => dbContext.Database.DataAdapterFill(sql, model, type);

        public virtual async Task<DataSet> DataAdapterFillAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await dbContext.Database.DataAdapterFillAsync(sql, model, type, cancellationToken);

        public virtual int ExecuteNonQuery(string sql, object model = null, CommandType type = CommandType.Text)
            => dbContext.Database.ExecuteNonQuery(sql, model, type);

        public virtual async Task<int> ExecuteNonQueryAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await dbContext.Database.ExecuteNonQueryAsync(sql, model, type, cancellationToken);

        public virtual T ExecuteScalar<T>(string sql, object model = null, CommandType type = CommandType.Text)
            => dbContext.Database.ExecuteScalar<T>(sql, model, type);

        public virtual async Task<T> ExecuteScalarAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await dbContext.Database.ExecuteScalarAsync<T>(sql, model, type, cancellationToken);

    }
}
