using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using XUCore.Extensions.Datas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.Common;
using XUCore.Extensions;
using System.Linq;

namespace XUCore.NetCore.Data.DbService
{

    /// <summary>
    /// sql server的仓库
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class MsSqlRepository<TEntity> : DbBaseRepository<TEntity>, IMsSqlRepository<TEntity> where TEntity : class, new()
    {
        public MsSqlRepository(IBaseContext context) : base(context) { }

        public override int ExecuteSql(string sql, params IDataParameter[] parameters)
        {
            parameters = parameters == null ? new SqlParameter[0] { } : parameters;
            var db = this._context as DbContext;
            return db.Database.ExecuteSqlRaw(sql, parameters);
        }

        public override T Select<T>(string sql, CommandType type, params IDataParameter[] parameters)
        {
            var res = SelectList<T>(sql, type, parameters);

            return res.Count > 0 ? res[0] : null;
        }

        public override IList<T> SelectList<T>(string sql, CommandType type, params IDataParameter[] parameters)
        {
            return SelectList(sql, type, parameters).ToList<T>();
        }

        public override DataTable SelectList(string sql, CommandType type, params IDataParameter[] parameters)
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

        public override DataSet SelectDataSet(string sql, CommandType type, params IDataParameter[] parameters)
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

        public override int ExecuteAdoNet(string sql, CommandType type, params IDataParameter[] parameters)
            => ExecuteAdoNet(sql, type, null, parameters);
        public override int ExecuteAdoNet(string sql, CommandType type, IDbTransaction dbTransaction, params IDataParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    if (dbTransaction != null)
                        cmd.Transaction = dbTransaction as SqlTransaction;
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

        public override IDataParameter GetParameter(string paramterName, object value)
            => new SqlParameter(paramterName, value);

        public override IDataParameter[] GetParameters(params (string paramterName, object value)[] paramters)
            => paramters?.ToMap(c => new SqlParameter(c.paramterName, c.value)).ToArray();
    }
}
