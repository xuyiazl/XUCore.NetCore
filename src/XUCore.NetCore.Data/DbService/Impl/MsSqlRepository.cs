using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using XUCore.Extensions.Datas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XUCore.NetCore.Data.DbService
{

    /// <summary>
    /// sql server的仓库
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MsSqlRepository<T> : DbBaseRepository<T> where T : class, new()
    {
        public MsSqlRepository(IBaseContext context) : base(context) { }

        #region mssql专有的ado执行
        public int ExecuteSql(string sql, params SqlParameter[] parameters)
        {
            parameters = parameters == null ? new SqlParameter[0] { } : parameters;
            var db = this._context as DbContext;
            return db.Database.ExecuteSqlRaw(sql, parameters);
        }

        public TEntity Select<TEntity>(string sql, CommandType type, params SqlParameter[] parameters) where TEntity : class, new()
        {
            var res = SelectList<TEntity>(sql, type, parameters);

            return res.Count > 0 ? res[0] : null;
        }

        public IList<TEntity> SelectList<TEntity>(string sql, CommandType type, params SqlParameter[] parameters) where TEntity : class, new()
        {
            return SelectList(sql, type, parameters).ToList<TEntity>();
        }

        public DataTable SelectList(string sql, CommandType type, params SqlParameter[] parameters)
        {
            parameters = parameters == null ? new SqlParameter[0] { } : parameters;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                    SqlDataAdapter dp = new SqlDataAdapter(cmd);
                    dp.Fill(ds);
                }
                return ds.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;//此处处理异常
            }

        }

        public DataSet SelectDataSet(string sql, CommandType type, params SqlParameter[] parameters)
        {
            parameters = parameters == null ? new SqlParameter[0] { } : parameters;
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                    SqlDataAdapter dp = new SqlDataAdapter(cmd);
                    dp.Fill(ds);
                }
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;//此处处理异常
            }
        }

        public int ExecuteAdoNet(string sql, CommandType type, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw ex;//此处处理异常
            }
        }

        #endregion
    }
}
