using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XUCore.NetCore.Data.DbService
{
    /// <summary>
    /// 定义SqlServer仓库
    /// </summary>
    public interface IMsSqlRepository<T> : IBaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// 通过EF执行原生SQL 返回影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Int32 ExecuteSql(string sql, params SqlParameter[] parameters);
        /// <summary>
        /// 通过ADO.NET执行SQL 返回查询结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        TEntity Select<TEntity>(string sql, CommandType type, params SqlParameter[] parameters) where TEntity : class, new();
        /// <summary>
        /// 通过ADO.NET执行SQL 返回查询结果集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IList<TEntity> SelectList<TEntity>(string sql, CommandType type, params SqlParameter[] parameters) where TEntity : class, new();
        /// <summary>
        /// 通过ADO.NET执行SQL 返回查询结果集合(DataTable)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable SelectList(string sql, CommandType type, params SqlParameter[] parameters);
        /// <summary>
        /// 通过ADO.NET执行SQL返回数据集(DataSet);
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataSet SelectDataSet(string sql, CommandType type, params SqlParameter[] parameters);
        /// <summary>
        /// 通过原生执行ADONET查询操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        int ExecuteAdoNet(string sql, CommandType type, params SqlParameter[] parameters);


        #region 增加mssql下面的bulk处理操作组件，仅支持mssql的实现的时候才支持该方法，未来一段时间mysql 都不会支持该方法
        /// <summary>
        /// 执行批量增加操作
        /// </summary>
        /// <param name="entities"></param>
        //void BulkInsertExtensions(IList<T> entities);

        #endregion

    }
}
