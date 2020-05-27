using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XUCore.Extensions.Datas;

namespace XUCore.NetCore.Data.DbService
{

    public class MySqlRepository<T> : DbBaseRepository<T> where T : class, new()
    {
        public MySqlRepository(IBaseContext context) : base(context) { }

        public int ExecuteSql(string sql, params MySqlParameter[] parameters)
        {
            parameters = parameters == null ? new MySqlParameter[0] { } : parameters;
            var db = this._context as DbContext;
            return db.Database.ExecuteSqlRaw(sql, parameters);
        }

        public TEntity Select<TEntity>(string sql, CommandType type, params MySqlParameter[] parameters) where TEntity : class, new()
        {
            var res = SelectList<TEntity>(sql, type, parameters);

            return res.Count > 0 ? res[0] : null;
        }

        public IList<TEntity> SelectList<TEntity>(string sql, CommandType type, params MySqlParameter[] parameters) where TEntity : class, new()
        {
            return SelectList(sql, type, parameters).ToList<TEntity>();
        }

        public DataTable SelectList(string sql, CommandType type, params MySqlParameter[] parameters)
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

        public DataSet SelectDataSet(string sql, CommandType type, params MySqlParameter[] parameters)
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

        public int ExecuteAdoNet(string sql, CommandType type, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
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

    }
}
