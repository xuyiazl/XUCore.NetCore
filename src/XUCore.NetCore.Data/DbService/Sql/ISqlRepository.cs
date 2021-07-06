using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{
    /// <summary>
    /// sql仓储
    /// </summary>
    public interface ISqlRepository
    {
        /// <summary>
        /// SQL查询并返回第一条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <returns></returns>
        TEntity SqlFirstOrDefault<TEntity>(string sql, object model = null, CommandType type = CommandType.Text) where TEntity : class, new();
        /// <summary>
        /// SQL查询并返回第一条记录
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TEntity> SqlFirstOrDefaultAsync<TEntity>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <returns></returns>
        IList<TEntity> SqlQuery<TEntity>(string sql, object model = null, CommandType type = CommandType.Text) where TEntity : class, new();
        /// <summary>
        /// SQL查询
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<TEntity>> SqlQueryAsync<TEntity>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default) where TEntity : class, new();
        /// <summary>
        /// SQL查询，返回DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <returns></returns>
        DataTable SqlReader(string sql, object model = null, CommandType type = CommandType.Text);
        /// <summary>
        /// SQL查询，返回DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DataTable> SqlReaderAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default);
        /// <summary>
        /// SQL查询，返回DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <returns></returns>
        DataSet SqlQueries(string sql, object model = null, CommandType type = CommandType.Text);
        /// <summary>
        /// SQL查询，返回DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DataSet> SqlQueriesAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default);
        /// <summary>
        /// 执行SQL，执行一个SqlCommand,该命令返回受操作影响的行数,该命令主要用于确定操作是否成功
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <returns></returns>
        int SqlNonQuery(string sql, object model = null, CommandType type = CommandType.Text);
        /// <summary>
        /// 执行SQL，执行一个SqlCommand,该命令返回受操作影响的行数,该命令主要用于确定操作是否成功
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SqlNonQueryAsync(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default);
        /// <summary>
        /// 执行SQL，并返回查询所返回的结果集中第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <returns></returns>
        T SqlScalar<T>(string sql, object model = null, CommandType type = CommandType.Text);
        /// <summary>
        /// 执行SQL，并返回查询所返回的结果集中第一行的第一列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="model">参数，匿名对象，模型，字典，DbParamter集合</param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> SqlScalarAsync<T>(string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default);

    }
}
