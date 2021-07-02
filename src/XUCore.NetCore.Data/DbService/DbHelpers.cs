using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Helpers;

namespace XUCore.NetCore.Data.DbService
{
    internal static class DbHelpers
    {


        public static DataTable ExecuteReader(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType type = CommandType.Text)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, _) = databaseFacade.PrepareDbCommand(sql, model, type);

            // 读取数据
            using var dbDataReader = dbCommand.ExecuteReader(CommandBehavior.Default);

            // 填充到 DataTable
            using var dataTable = new DataTable();
            dataTable.Load(dbDataReader);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return dataTable;
        }

        public static async Task<DataTable> ExecuteReaderAsync(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, _) = await databaseFacade.PrepareDbCommandAsync(sql, model, type, cancellationToken);

            // 读取数据
            using var dbDataReader = dbCommand.ExecuteReader(CommandBehavior.Default);

            // 填充到 DataTable
            using var dataTable = new DataTable();
            dataTable.Load(dbDataReader);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return dataTable;
        }

        public static DataSet DataAdapterFill(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType type = CommandType.Text)
        {
            // 初始化数据库连接对象、数据库命令对象和数据库适配器对象
            var (_, dbCommand, dbDataAdapter) = databaseFacade.PrepareDbDataAdapter(sql, model, type);

            // 填充DataSet
            using var dataSet = new DataSet();
            dbDataAdapter.Fill(dataSet);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return dataSet;
        }
        public static async Task<DataSet> DataAdapterFillAsync(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 初始化数据库连接对象、数据库命令对象和数据库适配器对象
            var (_, dbCommand, dbDataAdapter) = await databaseFacade.PrepareDbDataAdapterAsync(sql, model, type, cancellationToken);

            // 填充DataSet
            using var dataSet = new DataSet();
            dbDataAdapter.Fill(dataSet);

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return dataSet;
        }

        public static int ExecuteNonQuery(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType type = CommandType.Text)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, _) = databaseFacade.PrepareDbCommand(sql, model, type);

            // 执行返回受影响行数
            var rowEffects = dbCommand.ExecuteNonQuery();

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return rowEffects;
        }
        public static async Task<int> ExecuteNonQueryAsync(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, _) = await databaseFacade.PrepareDbCommandAsync(sql, model, type, cancellationToken);

            // 执行返回受影响行数
            var rowEffects = dbCommand.ExecuteNonQuery();

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return rowEffects;
        }

        public static T ExecuteScalar<T>(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType type = CommandType.Text)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, _) = databaseFacade.PrepareDbCommand(sql, model, type);

            // 执行返回单行单列的值
            var result = dbCommand.ExecuteScalar();

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return result != DBNull.Value ? Conv.To<T>(result) : default;
        }

        public static async Task<T> ExecuteScalarAsync<T>(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType type = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 初始化数据库连接对象和数据库命令对象
            var (_, dbCommand, _) = await databaseFacade.PrepareDbCommandAsync(sql, model, type, cancellationToken);

            // 执行返回单行单列的值
            var result = dbCommand.ExecuteScalar();

            // 清空命令参数
            dbCommand.Parameters.Clear();

            return result != DBNull.Value ? Conv.To<T>(result) : default;
        }


        internal static (DbConnection dbConnection, DbCommand dbCommand, DbParameter[] dbParameters) PrepareDbCommand(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType commandType = CommandType.Text)
        {
            // 创建数据库连接对象及数据库命令对象
            var (dbConnection, dbCommand) = databaseFacade.CreateDbCommand(sql, commandType);

            var dbParameters = ConvertToDbParameters(model, dbCommand);

            SetDbParameters(dbCommand, dbParameters);

            OpenConnection(databaseFacade, dbConnection);

            return (dbConnection, dbCommand, dbParameters);
        }

        internal static async Task<(DbConnection dbConnection, DbCommand dbCommand, DbParameter[] dbParameters)> PrepareDbCommandAsync(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 创建数据库连接对象及数据库命令对象
            var (dbConnection, dbCommand) = databaseFacade.CreateDbCommand(sql, commandType);

            var dbParameters = ConvertToDbParameters(model, dbCommand);

            SetDbParameters(dbCommand, dbParameters);

            await OpenConnectionAsync(databaseFacade, dbConnection, cancellationToken);

            return (dbConnection, dbCommand, dbParameters);
        }

        internal static (DbConnection dbConnection, DbCommand dbCommand, DbDataAdapter dbDataAdapter) PrepareDbDataAdapter(this DatabaseFacade databaseFacade,
            string sql, object model = null, CommandType commandType = CommandType.Text)
        {
            // 创建数据库连接对象、数据库命令对象和数据库适配器对象
            var (dbConnection, dbCommand, dbDataAdapter) = databaseFacade.CreateDbDataAdapter(sql, commandType);

            var dbParameters = ConvertToDbParameters(model, dbCommand);

            SetDbParameters(dbCommand, dbParameters);

            // 打开数据库连接
            OpenConnection(databaseFacade, dbConnection);

            // 返回
            return (dbConnection, dbCommand, dbDataAdapter);
        }

        internal static async Task<(DbConnection dbConnection, DbCommand dbCommand, DbDataAdapter dbDataAdapter)> PrepareDbDataAdapterAsync(this DatabaseFacade databaseFacade,
         string sql, object model = null, CommandType commandType = CommandType.Text, CancellationToken cancellationToken = default)
        {
            // 创建数据库连接对象、数据库命令对象和数据库适配器对象
            var (dbConnection, dbCommand, dbDataAdapter) = databaseFacade.CreateDbDataAdapter(sql, commandType);

            var dbParameters = ConvertToDbParameters(model, dbCommand);

            SetDbParameters(dbCommand, dbParameters);

            // 打开数据库连接
            await OpenConnectionAsync(databaseFacade, dbConnection, cancellationToken);

            // 返回
            return (dbConnection, dbCommand, dbDataAdapter);
        }

        private static (DbConnection dbConnection, DbCommand dbCommand, DbDataAdapter dbDataAdapter) CreateDbDataAdapter(this DatabaseFacade databaseFacade, string sql, CommandType commandType = CommandType.Text)
        {
            // 获取数据库连接字符串
            var dbConnection = databaseFacade.GetDbConnection();

            // 解析数据库提供器
            var profiledDbProviderFactory = DbProviderFactories.GetFactory(dbConnection);

            // 创建数据库连接对象及数据库命令对象
            var (_dbConnection, dbCommand) = databaseFacade.CreateDbCommand(sql, commandType);
            dbConnection = _dbConnection;

            // 创建数据适配器并设置查询命令对象
            var dbDataAdapter = profiledDbProviderFactory.CreateDataAdapter();
            if (dbDataAdapter != null)
            {
                dbDataAdapter.SelectCommand = dbCommand;
            }

            // 返回
            return (dbConnection, dbCommand, dbDataAdapter);
        }

        private static (DbConnection dbConnection, DbCommand dbCommand) CreateDbCommand(this DatabaseFacade databaseFacade, string sql, CommandType commandType = CommandType.Text)
        {
            // 判断是否启用 MiniProfiler 组件，如果有，则包装链接
            var dbConnection = databaseFacade.GetDbConnection();

            // 创建数据库命令对象
            var dbCommand = dbConnection.CreateCommand();

            // 设置基本参数
            dbCommand.Transaction = databaseFacade.CurrentTransaction?.GetDbTransaction();
            dbCommand.CommandType = commandType;
            dbCommand.CommandText = sql;

            // 设置超时
            var commandTimeout = databaseFacade.GetCommandTimeout();
            if (commandTimeout != null) dbCommand.CommandTimeout = commandTimeout.Value;

            // 返回
            return (dbConnection, dbCommand);
        }
        private static void OpenConnection(DatabaseFacade databaseFacade, DbConnection dbConnection)
        {
            // 判断连接字符串是否关闭，如果是，则开启
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }
        }
        private static async Task OpenConnectionAsync(DatabaseFacade databaseFacade, DbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            // 判断连接字符串是否关闭，如果是，则开启
            if (dbConnection.State == ConnectionState.Closed)
            {
                await dbConnection.OpenAsync(cancellationToken);
            }
        }

        /// <summary>
        /// 将模型转为 DbParameter 集合
        /// </summary>
        /// <param name="model">参数模型</param>
        /// <param name="dbCommand">数据库命令对象</param>
        /// <returns></returns>
        internal static DbParameter[] ConvertToDbParameters(object model, DbCommand dbCommand)
        {
            var modelType = model?.GetType();

            // 处理字典类型参数
            if (modelType == typeof(Dictionary<string, object>)) return ConvertToDbParameters((Dictionary<string, object>)model, dbCommand);

            var dbParameters = new List<DbParameter>();
            if (model == null || !modelType.IsClass) return dbParameters.ToArray();

            // 获取所有公开实例属性
            var properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (properties.Length == 0) return dbParameters.ToArray();

            // 遍历所有属性
            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(model) ?? DBNull.Value;

                // 创建命令参数
                var dbParameter = dbCommand.CreateParameter();

                dbParameter.ParameterName = property.Name;
                dbParameter.Value = propertyValue;
                dbParameters.Add(dbParameter);
            }

            return dbParameters.ToArray();
        }

        /// <summary>
        /// 将字典转换成命令参数
        /// </summary>
        /// <param name="keyValues">字典</param>
        /// <param name="dbCommand">数据库命令对象</param>
        /// <returns></returns>
        internal static DbParameter[] ConvertToDbParameters(Dictionary<string, object> keyValues, DbCommand dbCommand)
        {
            var dbParameters = new List<DbParameter>();
            if (keyValues == null || keyValues.Count == 0) return dbParameters.ToArray();

            foreach (var key in keyValues.Keys)
            {
                var value = keyValues[key] ?? DBNull.Value;

                // 创建命令参数
                var dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = key;
                dbParameter.Value = value;
                dbParameters.Add(dbParameter);
            }

            return dbParameters.ToArray();
        }
        /// <summary>
        /// 设置数据库命令对象参数
        /// </summary>
        /// <param name="dbCommand">数据库命令对象</param>
        /// <param name="parameters">命令参数</param>
        internal static void SetDbParameters(DbCommand dbCommand, DbParameter[] parameters = null)
        {
            if (parameters == null || parameters.Length == 0) return;

            // 添加命令参数前缀
            foreach (var parameter in parameters)
            {
                parameter.ParameterName = DbHelpers.FixSqlParameterPlaceholder(parameter.ParameterName);
                dbCommand.Parameters.Add(parameter);
            }
        }
        /// <summary>
        /// 修正不同数据库命令参数前缀不一致问题
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        internal static string FixSqlParameterPlaceholder(string parameterName)
        {
            //var placeholder = !DbProvider.IsDatabaseFor(providerName, DbProvider.Oracle) ? "@" : ":";
            if (parameterName.StartsWith("@") || parameterName.StartsWith(":"))
            {
                parameterName = parameterName[1..];
            }

            //return isFixed ? placeholder + parameterName : parameterName;
            return "@" + parameterName;
        }
    }
}
