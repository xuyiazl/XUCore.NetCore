using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{
    public interface ISqlRepository
    {
        TEntity SqlFirstOrDefault<TEntity>(string sql, object model = null, CommandType type = CommandType.Text) where TEntity : class, new();

        Task<TEntity> SqlFirstOrDefaultAsync<TEntity>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default) where TEntity : class, new();

        IList<TEntity> SqlQuery<TEntity>(string sql, object model = null, CommandType type = CommandType.Text) where TEntity : class, new();

        Task<IList<TEntity>> SqlQueryAsync<TEntity>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default) where TEntity : class, new();

        DataTable ExecuteReader(string sql, object model = null, CommandType type = CommandType.Text);

        Task<DataTable> ExecuteReaderAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default);

        DataSet DataAdapterFill(string sql, object model = null, CommandType type = CommandType.Text);

        Task<DataSet> DataAdapterFillAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default);

        int ExecuteNonQuery(string sql, object model = null, CommandType type = CommandType.Text);

        Task<int> ExecuteNonQueryAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default);

        T ExecuteScalar<T>(string sql, object model = null, CommandType type = CommandType.Text);

        Task<T> ExecuteScalarAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default);

    }
}
