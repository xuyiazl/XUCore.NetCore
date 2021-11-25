using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions.Datas;

namespace XUCore.NetCore.Data
{
    public class SqlRepository : ISqlRepository
    {
        private readonly IDbContext dbContext;
        public SqlRepository(IDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual T SqlFirst<T>(string sql, object model = null, CommandType type = CommandType.Text)
        {
            return SqlQuery<T>(sql, model, type).FirstOrDefault();
        }

        public virtual async Task<T> SqlFirstAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
        {
            var res = await SqlQueryAsync<T>(sql, model, type, cancellationToken);

            return res.FirstOrDefault();
        }

        public virtual IList<T> SqlQuery<T>(string sql, object model = null, CommandType type = CommandType.Text)
        {
            return SqlReader(sql, model, type).ToList<T>();
        }

        public virtual async Task<IList<T>> SqlQueryAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
        {
            var res = await SqlReaderAsync(sql, model, type, cancellationToken);

            return res.ToList<T>();
        }

        public virtual DataTable SqlReader(string sql, object model = null, CommandType type = CommandType.Text)
            => dbContext.Database.ExecuteReader(sql, model, type);

        public virtual async Task<DataTable> SqlReaderAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await dbContext.Database.ExecuteReaderAsync(sql, model, type, cancellationToken);

        public virtual DataSet SqlQueries(string sql, object model = null, CommandType type = CommandType.Text)
            => dbContext.Database.DataAdapterFill(sql, model, type);

        public virtual async Task<DataSet> SqlQueriesAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await dbContext.Database.DataAdapterFillAsync(sql, model, type, cancellationToken);

        public virtual int SqlNonQuery(string sql, object model = null, CommandType type = CommandType.Text)
            => dbContext.Database.ExecuteNonQuery(sql, model, type);

        public virtual async Task<int> SqlNonQueryAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await dbContext.Database.ExecuteNonQueryAsync(sql, model, type, cancellationToken);

        public virtual T SqlScalar<T>(string sql, object model = null, CommandType type = CommandType.Text)
            => dbContext.Database.ExecuteScalar<T>(sql, model, type);

        public virtual async Task<T> SqlScalarAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
            => await dbContext.Database.ExecuteScalarAsync<T>(sql, model, type, cancellationToken);

    }
}
