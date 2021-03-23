using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using XUCore.Extensions;
using XUCore.Extensions.Datas;

namespace XUCore.NetCore.Data.DbService
{

    public class MySqlRepository<TEntity> : DbRepository<TEntity>, IMySqlRepository<TEntity> where TEntity : class, new()
    {
        public MySqlRepository(IDbContext context) : base(context) { }

        public override int ExecuteSql(string sql, params IDataParameter[] parameters)
        {
            parameters = parameters == null ? new MySqlParameter[0] { } : parameters;
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
            parameters = parameters == null ? new MySqlParameter[0] { } : parameters;
            DataSet ds = new DataSet();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;
                    foreach (MySqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                    MySqlDataAdapter dp = new MySqlDataAdapter(cmd);
                    dp.Fill(ds);
                }
                return ds.Tables[0];
            }
            catch (MySqlException ex)
            {
                throw ex;//此处处理异常
            }

        }

        public override DataSet SelectDataSet(string sql, CommandType type, params IDataParameter[] parameters)
        {
            parameters = parameters == null ? new MySqlParameter[0] { } : parameters;
            DataSet ds = new DataSet();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;
                    foreach (MySqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                    MySqlDataAdapter dp = new MySqlDataAdapter(cmd);
                    dp.Fill(ds);
                }
                return ds;
            }
            catch (MySqlException ex)
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
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    if (dbTransaction != null)
                        cmd.Transaction = dbTransaction as MySqlTransaction;
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;
                    foreach (MySqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                throw ex;//此处处理异常
            }

        }

        public override IDataParameter GetParameter(string paramterName, object value)
            => new MySqlParameter(paramterName, value);

        public override IDataParameter[] GetParameters(params (string paramterName, object value)[] paramters)
            => paramters?.ToMap(c => new MySqlParameter(c.paramterName, c.value)).ToArray();
    }
}
