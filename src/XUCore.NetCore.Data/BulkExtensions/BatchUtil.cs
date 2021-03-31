using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.BulkExtensions
{
    public static class BatchUtil
    {
        static readonly int SelectStatementLength = "SELECT".Length;

        // In comment are Examples of how SqlQuery is changed for Sql Batch

        // SELECT [a].[Column1], [a].[Column2], .../r/n
        // FROM [Table] AS [a]/r/n
        // WHERE [a].[Column] = FilterValue
        // --
        // DELETE [a]
        // FROM [Table] AS [a]
        // WHERE [a].[Columns] = FilterValues
        public static (string, List<object>) GetSqlDelete<T>(IQueryable<T> query, DbContext context) where T : class
        {
            (string sql, string tableAlias, string tableAliasSufixAs, string topStatement, IEnumerable<object> innerParameters) = GetBatchSql(query, context, isUpdate: false);

            innerParameters = ReloadSqlParameters(context, innerParameters.ToList()); // Sqlite requires SqliteParameters
            tableAlias = (GetDatabaseType(context) == DbServer.SqlServer) ? $"[{tableAlias}]" : tableAlias;
            var dbType = GetDatabaseType(context);
            if (dbType == DbServer.MySql)
            {
                var resultQuery = $"DELETE {Regex.Replace(sql, @$"AS|{tableAlias}|\.", "")}";
                return (resultQuery, new List<object>(innerParameters));
            }
            else
            {
                var resultQuery = $"DELETE {topStatement}{tableAlias}{sql}";
                return (resultQuery, new List<object>(innerParameters));
            }
        }

        // SELECT [a].[Column1], [a].[Column2], .../r/n
        // FROM [Table] AS [a]/r/n
        // WHERE [a].[Column] = FilterValue
        // --
        // UPDATE [a] SET [UpdateColumns] = N'updateValues'
        // FROM [Table] AS [a]
        // WHERE [a].[Columns] = FilterValues
        public static (string, List<object>) GetSqlUpdate<T>(IQueryable<T> query, DbContext context, T updateValues, List<string> updateColumns) where T : class, new()
        {
            (string sql, string tableAlias, string tableAliasSufixAs, string topStatement, IEnumerable<object> innerParameters) = GetBatchSql(query, context, isUpdate: true);
            var sqlParameters = new List<object>(innerParameters);

            var dbType = GetDatabaseType(context);

            string sqlSET = GetSqlSetSegment(context, dbType, updateValues, updateColumns, sqlParameters);

            sqlParameters = ReloadSqlParameters(context, sqlParameters); // Sqlite requires SqliteParameters

            if (dbType == DbServer.MySql)
            {
                sql = sql.Substring(sql.IndexOf(Environment.NewLine) + 6, sql.Length - (sql.IndexOf(Environment.NewLine) + 6));
                string tbName = sql.Substring(0, sql.IndexOf(Environment.NewLine));
                string where = sql.Substring(sql.IndexOf(Environment.NewLine), sql.Length - sql.IndexOf(Environment.NewLine));
                sqlSET = Regex.Replace(sqlSET, @$"\[", $"{tableAlias}.`");
                sqlSET = Regex.Replace(sqlSET, @$"\]", $"`");
                var resultQuery = $"UPDATE {tbName} {sqlSET} {where}";
                return (resultQuery, sqlParameters);
            }
            else
            {
                var resultQuery = $"UPDATE {topStatement}{tableAlias}{tableAliasSufixAs} {sqlSET}{sql}";
                return (resultQuery, sqlParameters);
            }
        }

        /// <summary>
        /// get Update Sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static (string, List<object>) GetSqlUpdate<T>(IQueryable<T> query, DbContext context, Expression<Func<T, T>> expression) where T : class
        {
            (string sql, string tableAlias, string tableAliasSufixAs, string topStatement, IEnumerable<object> innerParameters) = GetBatchSql(query, context, isUpdate: true);
            var sqlColumns = new StringBuilder();
            var sqlParameters = new List<object>(innerParameters);
            var columnNameValueDict = TableInfo.CreateInstance(GetDbContext(query), new List<T>(), OperationType.Read, new BulkConfig()).PropertyColumnNamesDict;
            var dbType = GetDatabaseType(context);
            CreateUpdateBody(columnNameValueDict, tableAlias, expression.Body, dbType, ref sqlColumns, ref sqlParameters);

            sqlParameters = ReloadSqlParameters(context, sqlParameters); // Sqlite requires SqliteParameters
            sqlColumns = (dbType == DbServer.SqlServer) ? sqlColumns : sqlColumns.Replace($"[{tableAlias}].", "");

            if (dbType == DbServer.MySql)
            {
                sql = sql.Substring(sql.IndexOf(Environment.NewLine) + 6, sql.Length - (sql.IndexOf(Environment.NewLine) + 6));
                string tbName = sql.Substring(0, sql.IndexOf(Environment.NewLine));
                string where = sql.Substring(sql.IndexOf(Environment.NewLine), sql.Length - sql.IndexOf(Environment.NewLine));
                var resultQuery = $"UPDATE {tbName} SET {sqlColumns} {where}";
                return (resultQuery, sqlParameters);
            }
            else
            {
                var resultQuery = $"UPDATE {topStatement}{tableAlias}{tableAliasSufixAs} SET {sqlColumns} {sql}";
                return (resultQuery, sqlParameters);
            }
        }

        public static List<object> ReloadSqlParameters(DbContext context, List<object> sqlParameters)
        {
            DbServer databaseType = GetDatabaseType(context);
            if (databaseType == DbServer.Sqlite)
            {
                var sqlParametersReloaded = new List<object>();
                foreach (var parameter in sqlParameters)
                {
                    var sqlParameter = (SqlParameter)parameter;
                    sqlParametersReloaded.Add(new SqliteParameter(sqlParameter.ParameterName, sqlParameter.Value));
                }
                return sqlParametersReloaded;
            }
            else // for SqlServer return original
            {
                return sqlParameters;
            }
        }

        public static (string, string, string, string, IEnumerable<object>) GetBatchSql<T>(IQueryable<T> query, DbContext context, bool isUpdate) where T : class
        {
            DbServer databaseType = GetDatabaseType(context);

            var (sqlQuery, innerParameters) = query.ToParametrizedSql(databaseType);

            string tableAlias = string.Empty;
            string tableAliasSufixAs = string.Empty;
            string topStatement = string.Empty;
            if (databaseType != DbServer.Sqlite) // when Sqlite and Deleted metod tableAlias is Empty: ""
            {
                string escapeSymbolEnd = (databaseType == DbServer.SqlServer) ? "]" : "."; // SqlServer : PostrgeSql;
                string escapeSymbolStart = (databaseType == DbServer.SqlServer) ? "[" : " "; // SqlServer : PostrgeSql;
                string tableAliasEnd = sqlQuery.Substring(SelectStatementLength, sqlQuery.IndexOf(escapeSymbolEnd) - SelectStatementLength); // " TOP(10) [table_alias" / " [table_alias" : " table_alias"
                int tableAliasStartIndex = tableAliasEnd.IndexOf(escapeSymbolStart);
                tableAlias = tableAliasEnd.Substring(tableAliasStartIndex + escapeSymbolStart.Length); // "table_alias"
                topStatement = tableAliasEnd.Substring(0, tableAliasStartIndex).TrimStart(); // "TOP(10) " / if TOP not present in query this will be a Substring(0,0) == ""
            }

            int indexFROM = sqlQuery.IndexOf(Environment.NewLine);
            string sql = sqlQuery.Substring(indexFROM, sqlQuery.Length - indexFROM);
            sql = sql.Contains("{") ? sql.Replace("{", "{{") : sql; // Curly brackets have to be escaped:
            sql = sql.Contains("}") ? sql.Replace("}", "}}") : sql; // https://github.com/aspnet/EntityFrameworkCore/issues/8820

            if (isUpdate && databaseType == DbServer.Sqlite)
            {
                var match = Regex.Match(sql, @"FROM (""[^""]+"")( AS ""[^""]+"")");
                tableAlias = match.Groups[1].Value;
                tableAliasSufixAs = match.Groups[2].Value;
                sql = sql.Substring(match.Index + match.Length);
            }

            return (sql, tableAlias, tableAliasSufixAs, topStatement, innerParameters);
        }

        public static string GetSqlSetSegment<T>(DbContext context, DbServer dbType, T updateValues, List<string> updateColumns, List<object> parameters) where T : class, new()
        {
            var tableInfo = TableInfo.CreateInstance<T>(context, new List<T>(), OperationType.Read, new BulkConfig());
            string sql = string.Empty;
            Type updateValuesType = typeof(T);
            var defaultValues = new T();
            foreach (var propertyNameColumnName in tableInfo.PropertyColumnNamesDict)
            {
                string propertyName = propertyNameColumnName.Key;
                string columnName = propertyNameColumnName.Value;
                var pArray = propertyName.Split(new char[] { '.' });
                Type lastType = updateValuesType;
                PropertyInfo property = lastType.GetProperty(pArray[0]);
                if (property != null)
                {
                    object propertyUpdateValue = property.GetValue(updateValues);
                    object propertyDefaultValue = property.GetValue(defaultValues);
                    for (int i = 1; i < pArray.Length; i++)
                    {
                        lastType = property.PropertyType;
                        property = lastType.GetProperty(pArray[i]);
                        propertyUpdateValue = propertyUpdateValue != null ? property.GetValue(propertyUpdateValue) : propertyUpdateValue;
                        var lastDefaultValues = lastType.Assembly.CreateInstance(lastType.FullName);
                        propertyDefaultValue = property.GetValue(lastDefaultValues);
                    }

                    if (tableInfo.ConvertibleProperties.ContainsKey(columnName))
                    {
                        propertyUpdateValue = tableInfo.ConvertibleProperties[columnName].ConvertToProvider.Invoke(propertyUpdateValue);
                    }

                    bool isDifferentFromDefault = propertyUpdateValue != null && propertyUpdateValue?.ToString() != propertyDefaultValue?.ToString();
                    if (isDifferentFromDefault || (updateColumns != null && updateColumns.Contains(propertyName)))
                    {
                        sql += $"[{columnName}] = @{columnName}, ";
                        propertyUpdateValue = propertyUpdateValue ?? DBNull.Value;

                        if (dbType == DbServer.MySql)
                            parameters.Add(new MySqlParameter($"@{columnName}", propertyUpdateValue));
                        else
                            parameters.Add(new SqlParameter($"@{columnName}", propertyUpdateValue));
                    }
                }
            }
            if (String.IsNullOrEmpty(sql))
            {
                throw new InvalidOperationException("SET Columns not defined. If one or more columns should be updated to theirs default value use 'updateColumns' argument.");
            }
            sql = sql.Remove(sql.Length - 2, 2); // removes last excess comma and space: ", "
            return $"SET {sql}";
        }

        /// <summary>
        /// Recursive analytic expression 
        /// </summary>
        /// <param name="tableAlias"></param>
        /// <param name="expression"></param>
        /// <param name="sqlColumns"></param>
        /// <param name="sqlParameters"></param>
        public static void CreateUpdateBody(Dictionary<string, string> columnNameValueDict, string tableAlias, Expression expression, DbServer dbType, ref StringBuilder sqlColumns, ref List<object> sqlParameters)
        {
            if (expression is MemberInitExpression memberInitExpression)
            {
                foreach (var item in memberInitExpression.Bindings)
                {
                    if (item is MemberAssignment assignment)
                    {
                        if (columnNameValueDict.TryGetValue(assignment.Member.Name, out string value))
                        {
                            if (dbType == DbServer.MySql)
                                sqlColumns.Append($" {tableAlias}.`{value}`");
                            else
                                sqlColumns.Append($" [{tableAlias}].[{value}]");
                        }
                        else
                            sqlColumns.Append($" [{tableAlias}].[{assignment.Member.Name}]");

                        sqlColumns.Append(" =");

                        CreateUpdateBody(columnNameValueDict, tableAlias, assignment.Expression, dbType, ref sqlColumns, ref sqlParameters);

                        if (memberInitExpression.Bindings.IndexOf(item) < (memberInitExpression.Bindings.Count - 1))
                            sqlColumns.Append(" ,");
                    }
                }
            }
            else if (expression is MemberExpression memberExpression && memberExpression.Expression is ParameterExpression)
            {
                if (columnNameValueDict.TryGetValue(memberExpression.Member.Name, out string value))
                {
                    if (dbType == DbServer.MySql)
                        sqlColumns.Append($" {tableAlias}.`{value}`");
                    else
                        sqlColumns.Append($" [{tableAlias}].[{value}]");
                }
                else
                    sqlColumns.Append($" [{tableAlias}].[{memberExpression.Member.Name}]");
            }
            else if (expression is ConstantExpression constantExpression)
            {
                var parmName = $"param_{sqlParameters.Count}";
                if (dbType == DbServer.MySql)
                    sqlParameters.Add(new MySqlParameter(parmName, constantExpression.Value ?? DBNull.Value));
                else
                    sqlParameters.Add(new SqlParameter(parmName, constantExpression.Value ?? DBNull.Value));
                sqlColumns.Append($" @{parmName}");
            }
            else if (expression is UnaryExpression unaryExpression)
            {
                switch (unaryExpression.NodeType)
                {
                    case ExpressionType.Convert:
                        CreateUpdateBody(columnNameValueDict, tableAlias, unaryExpression.Operand, dbType, ref sqlColumns, ref sqlParameters);
                        break;
                    case ExpressionType.Not:
                        sqlColumns.Append(" ~");//this way only for SQL Server 
                        CreateUpdateBody(columnNameValueDict, tableAlias, unaryExpression.Operand, dbType, ref sqlColumns, ref sqlParameters);
                        break;
                    default: break;
                }
            }
            else if (expression is BinaryExpression binaryExpression)
            {
                CreateUpdateBody(columnNameValueDict, tableAlias, binaryExpression.Left, dbType, ref sqlColumns, ref sqlParameters);

                switch (binaryExpression.NodeType)
                {
                    case ExpressionType.Add:
                        sqlColumns.Append(dbType == DbServer.Sqlite && IsStringConcat(binaryExpression) ? " ||" : " +");
                        break;
                    case ExpressionType.Divide:
                        sqlColumns.Append(" /");
                        break;
                    case ExpressionType.Multiply:
                        sqlColumns.Append(" *");
                        break;
                    case ExpressionType.Subtract:
                        sqlColumns.Append(" -");
                        break;
                    case ExpressionType.And:
                        sqlColumns.Append(" &");
                        break;
                    case ExpressionType.Or:
                        sqlColumns.Append(" |");
                        break;
                    case ExpressionType.ExclusiveOr:
                        sqlColumns.Append(" ^");
                        break;
                    default: break;
                }

                CreateUpdateBody(columnNameValueDict, tableAlias, binaryExpression.Right, dbType, ref sqlColumns, ref sqlParameters);
            }
            else
            {
                var value = Expression.Lambda(expression).Compile().DynamicInvoke();
                var parmName = $"param_{sqlParameters.Count}";
                if (dbType == DbServer.MySql)
                    sqlParameters.Add(new MySqlParameter(parmName, value ?? DBNull.Value));
                else
                    sqlParameters.Add(new SqlParameter(parmName, value ?? DBNull.Value));
                sqlColumns.Append($" @{parmName}");
            }
        }

        public static DbContext GetDbContext(IQueryable query)
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var queryCompiler = typeof(EntityQueryProvider).GetField("_queryCompiler", bindingFlags).GetValue(query.Provider);
            var queryContextFactory = queryCompiler.GetType().GetField("_queryContextFactory", bindingFlags).GetValue(queryCompiler);

            var dependencies = typeof(RelationalQueryContextFactory).GetField("_dependencies", bindingFlags).GetValue(queryContextFactory);
            var queryContextDependencies = typeof(DbContext).Assembly.GetType(typeof(QueryContextDependencies).FullName);
            var stateManagerProperty = queryContextDependencies.GetProperty("StateManager", bindingFlags | BindingFlags.Public).GetValue(dependencies);
            var stateManager = (IStateManager)stateManagerProperty;

            return stateManager.Context;
        }

        public static DbServer GetDatabaseType(DbContext context)
        {
            if (context.Database.ProviderName.EndsWith(DbServer.Sqlite.ToString()))
                return DbServer.Sqlite;
            else if (context.Database.ProviderName.EndsWith(DbServer.MySql.ToString()))
                return DbServer.MySql;
            else
                return DbServer.SqlServer;
            //return context.Database.ProviderName.EndsWith(DbServer.Sqlite.ToString()) ? DbServer.Sqlite : DbServer.SqlServer;
        }

        internal static bool IsStringConcat(BinaryExpression binaryExpression)
        {
            var methodProperty = binaryExpression.GetType().GetProperty("Method");
            if (methodProperty == null)
            {
                return false;
            }
            var method = methodProperty.GetValue(binaryExpression) as MethodInfo;
            if (method == null)
            {
                return false;
            }
            return method.DeclaringType == typeof(string) && method.Name == nameof(string.Concat);
        }
    }
}
