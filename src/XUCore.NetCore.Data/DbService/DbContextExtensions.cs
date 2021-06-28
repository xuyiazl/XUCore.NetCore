using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XUCore.Extensions.Datas;
using XUCore.Helpers;

namespace XUCore.NetCore.Data.DbService
{
    public static class DbContextExtensions
    {
        public static int ExecuteSql(this IDbContext dbContext, string sql, params IDataParameter[] parameters)
        {
            return dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }

        public static TEntity SqlFirstOrDefault<TEntity>(this IDbContext dbContext, string sql, CommandType type, params IDataParameter[] parameters) where TEntity : new()
        {
            var res = SqlQuery<TEntity>(dbContext, sql, type, parameters);

            return res.Count > 0 ? res[0] : default;
        }

        public static IList<TEntity> SqlQuery<TEntity>(this IDbContext dbContext, string sql, CommandType type, params IDataParameter[] parameters) where TEntity : new()
        {
            return SqlDataTable(dbContext, sql, type, parameters).ToList<TEntity>();
        }

        public static DataTable SqlDataTable(this IDbContext dbContext, string sql, CommandType type, params IDataParameter[] parameters)
        {
            var ds = SqlDataSet(dbContext, sql, type, parameters);

            return ds.Tables[0];
        }

        public static DataSet SqlDataSet(this IDbContext dbContext, string sql, CommandType type, params IDataParameter[] parameters)
        {
            if (dbContext.Database.IsSqlServer())
                return SqlServerDataSet(dbContext, sql, type, parameters);

            else if (dbContext.Database.IsMySql())
                return MySqlDataSet(dbContext, sql, type, parameters);

            return null;
        }

        private static DataSet SqlServerDataSet(this IDbContext dbContext, string sql, CommandType type, params IDataParameter[] parameters)
        {
            try
            {
                DataSet ds = new DataSet();

                using (SqlConnection conn = new SqlConnection(dbContext.ConnectionStrings))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;

                    foreach (SqlParameter p in parameters)
                        cmd.Parameters.Add(p);

                    using (SqlDataAdapter dp = new SqlDataAdapter(cmd))
                        dp.Fill(ds);
                }
                return ds;
            }
            catch
            {
                throw;//此处处理异常
            }
        }

        private static DataSet MySqlDataSet(this IDbContext dbContext, string sql, CommandType type, params IDataParameter[] parameters)
        {
            try
            {
                DataSet ds = new DataSet();

                using (MySqlConnection conn = new MySqlConnection(dbContext.ConnectionStrings))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;

                    foreach (MySqlParameter p in parameters)
                        cmd.Parameters.Add(p);

                    using (MySqlDataAdapter dp = new MySqlDataAdapter(cmd))
                        dp.Fill(ds);
                }
                return ds;
            }
            catch
            {
                throw;//此处处理异常
            }
        }

        public static int ExecuteSql(this IDbContext dbContext, string sql, CommandType type, IDbTransaction transaction = null, params IDataParameter[] parameters)
        {
            if (dbContext.Database.IsSqlServer())
                return SqlServerExecuteSql(dbContext, sql, type, transaction, parameters);

            else if (dbContext.Database.IsMySql())
                return MySqlExecuteSql(dbContext, sql, type, transaction, parameters);

            return 0;
        }

        private static int SqlServerExecuteSql(this IDbContext dbContext, string sql, CommandType type, IDbTransaction transaction, params IDataParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.ConnectionStrings))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand();

                    if (transaction != null)
                        cmd.Transaction = transaction as SqlTransaction;

                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;

                    foreach (SqlParameter p in parameters)
                        cmd.Parameters.Add(p);

                    return cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;//此处处理异常
            }
        }

        private static int MySqlExecuteSql(this IDbContext dbContext, string sql, CommandType type, IDbTransaction transaction, params IDataParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(dbContext.ConnectionStrings))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand();

                    if (transaction != null)
                        cmd.Transaction = transaction as MySqlTransaction;

                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;

                    foreach (MySqlParameter p in parameters)
                        cmd.Parameters.Add(p);

                    return cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;//此处处理异常
            }
        }

        public static T ExecuteScalar<T>(this IDbContext dbContext, string sql, CommandType type, IDbTransaction transaction = null, params IDataParameter[] parameters)
        {
            if (dbContext.Database.IsSqlServer())
                return SqlServerExecuteScalar<T>(dbContext, sql, type, transaction, parameters);

            else if (dbContext.Database.IsMySql())
                return MySqlExecuteScalar<T>(dbContext, sql, type, transaction, parameters);

            return default;
        }

        private static T SqlServerExecuteScalar<T>(this IDbContext dbContext, string sql, CommandType type, IDbTransaction transaction, params IDataParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(dbContext.ConnectionStrings))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand();

                    if (transaction != null)
                        cmd.Transaction = transaction as SqlTransaction;

                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;

                    foreach (SqlParameter p in parameters)
                        cmd.Parameters.Add(p);

                    var obj = cmd.ExecuteScalar();

                    return Conv.To<T>(obj);
                }
            }
            catch
            {
                throw;//此处处理异常
            }
        }

        private static T MySqlExecuteScalar<T>(this IDbContext dbContext, string sql, CommandType type, IDbTransaction transaction, params IDataParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(dbContext.ConnectionStrings))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand();

                    if (transaction != null)
                        cmd.Transaction = transaction as MySqlTransaction;

                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = type;

                    foreach (MySqlParameter p in parameters)
                        cmd.Parameters.Add(p);

                    var obj = cmd.ExecuteScalar();

                    return Conv.To<T>(obj);
                }
            }
            catch
            {
                throw;//此处处理异常
            }
        }
    }
}
