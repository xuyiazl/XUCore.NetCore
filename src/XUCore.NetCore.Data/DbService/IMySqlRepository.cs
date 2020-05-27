using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XUCore.NetCore.Data.DbService
{

    public interface IMySqlRepository<T> : IBaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// 通过EF执行原生SQL 返回影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Int32 ExecuteSql(string sql, params MySqlParameter[] parameters);
        /// <summary>
        /// 通过ADO.NET执行SQL 返回查询结果
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        TEntity Select<TEntity>(string sql, CommandType type, params MySqlParameter[] parameters) where TEntity : class, new();
        /// <summary>
        /// 通过ADO.NET执行SQL 返回查询结果集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<TEntity> SelectList<TEntity>(string sql, CommandType type, params MySqlParameter[] parameters) where TEntity : class, new();
        /// <summary>
        /// 通过ADO.NET执行SQL 返回查询结果集合(DataTable)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable SelectList(string sql, CommandType type, params MySqlParameter[] parameters);
        /// <summary>
        /// 通过ADO.NET执行SQL返回数据集(DataSet);
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataSet SelectDataSet(string sql, CommandType type, params MySqlParameter[] parameters);
        /// <summary>
        /// 通过原生执行ADONET查询操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        int ExecuteAdoNet(string sql, CommandType type, params MySqlParameter[] parameters);
    }
}
