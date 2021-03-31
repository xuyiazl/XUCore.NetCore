using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using MySqlConnector;

namespace XUCore.NetCore.Data.BulkExtensions
{
    public static class IQueryableExtensions
    {
        public static (string, IEnumerable<DbParameter>) ToParametrizedSql<TEntity>(this IQueryable<TEntity> query, DbServer dbType) where TEntity : class
        {
            string relationalCommandCacheText = "_relationalCommandCache";
            string selectExpressionText = "_selectExpression";
            string querySqlGeneratorFactoryText = "_querySqlGeneratorFactory";
            string relationalQueryContextText = "_relationalQueryContext";

            string cannotGetText = "Cannot get";

            var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private(relationalCommandCacheText) as RelationalCommandCache;
            var queryContext = enumerator.Private<RelationalQueryContext>(relationalQueryContextText) ?? throw new InvalidOperationException($"{cannotGetText} {relationalQueryContextText}");
            var parameterValues = queryContext.ParameterValues;

            if (dbType == DbServer.MySql)
            {
                string sql;
                IList<MySqlParameter> parameters;
                if (relationalCommandCache != null)
                {
                    var command = relationalCommandCache.GetRelationalCommand(parameterValues);
                    var parameterNames = new HashSet<string>(command.Parameters.Select(p => p.InvariantName));
                    sql = command.CommandText;
                    parameters = parameterValues.Where(pv => parameterNames.Contains(pv.Key)).Select(pv => new MySqlParameter("@" + pv.Key, pv.Value)).ToList();
                }
                else
                {
                    SelectExpression selectExpression = enumerator.Private<SelectExpression>(selectExpressionText) ?? throw new InvalidOperationException($"{cannotGetText} {selectExpressionText}");
                    IQuerySqlGeneratorFactory factory = enumerator.Private<IQuerySqlGeneratorFactory>(querySqlGeneratorFactoryText) ?? throw new InvalidOperationException($"{cannotGetText} {querySqlGeneratorFactoryText}");

                    var sqlGenerator = factory.Create();
                    var command = sqlGenerator.GetCommand(selectExpression);
                    sql = command.CommandText;
                    parameters = parameterValues.Select(pv => new MySqlParameter("@" + pv.Key, pv.Value)).ToList();
                }

                return (sql, parameters);
            }
            else
            {
                string sql;
                IList<SqlParameter> parameters;
                if (relationalCommandCache != null)
                {
                    var command = relationalCommandCache.GetRelationalCommand(parameterValues);
                    var parameterNames = new HashSet<string>(command.Parameters.Select(p => p.InvariantName));
                    sql = command.CommandText;
                    parameters = parameterValues.Where(pv => parameterNames.Contains(pv.Key)).Select(pv => new SqlParameter("@" + pv.Key, pv.Value)).ToList();
                }
                else
                {
                    SelectExpression selectExpression = enumerator.Private<SelectExpression>(selectExpressionText) ?? throw new InvalidOperationException($"{cannotGetText} {selectExpressionText}");
                    IQuerySqlGeneratorFactory factory = enumerator.Private<IQuerySqlGeneratorFactory>(querySqlGeneratorFactoryText) ?? throw new InvalidOperationException($"{cannotGetText} {querySqlGeneratorFactoryText}");

                    var sqlGenerator = factory.Create();
                    var command = sqlGenerator.GetCommand(selectExpression);
                    sql = command.CommandText;
                    parameters = parameterValues.Select(pv => new SqlParameter("@" + pv.Key, pv.Value)).ToList();
                }

                return (sql, parameters);
            }
        }

        private static readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, bindingFlags)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, bindingFlags)?.GetValue(obj);

        public static string ToQuerySql<TEntity>(this IQueryable<TEntity> query, DbServer dbType) where TEntity : class
        {
            var (sql, paramters) = query.ToParametrizedSql(dbType);

            foreach (var parm in paramters)
            {
                var placeHolder = parm.ParameterName;
                var actualValue = GetActualValue(parm.Value);
                sql = sql.Replace(placeHolder, actualValue);
            }

            return sql;
        }

        private static string GetActualValue(object value)
        {
            var type = value.GetType();

            if (type.IsNumeric())
                return value.ToString();

            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                switch (type.Name)
                {
                    case nameof(DateTime):
                        return $"'{(DateTime)value:u}'";

                    case nameof(DateTimeOffset):
                        return $"'{(DateTimeOffset)value:u}'";
                }
            }

            return $"'{value}'";
        }

        private static bool IsNullable(this Type type)
        {
            return
                type != null &&
                type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool IsNumeric(this Type type)
        {
            if (IsNullable(type))
                type = Nullable.GetUnderlyingType(type);

            if (type == null || type.IsEnum)
                return false;

            return Type.GetTypeCode(type) switch
            {
                TypeCode.Byte => true,
                TypeCode.Decimal => true,
                TypeCode.Double => true,
                TypeCode.Int16 => true,
                TypeCode.Int32 => true,
                TypeCode.Int64 => true,
                TypeCode.SByte => true,
                TypeCode.Single => true,
                TypeCode.UInt16 => true,
                TypeCode.UInt32 => true,
                TypeCode.UInt64 => true,
                _ => false
            };
        }
    }
}
