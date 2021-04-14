﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;

namespace XUCore.Extensions
{
    public static class LinqExtensions
    {
        private static PropertyInfo GetPropertyInfo(Type objType, string name)
        {
            var properties = objType.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name.ToLower() == name.ToLower());
            if (matchedProperty == null)
            {
                throw new ArgumentException("name");
            }

            return matchedProperty;
        }

        private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
        {
            var paramExpr = Expression.Parameter(objType);
            var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);
            var expr = Expression.Lambda(propAccess, paramExpr);
            return expr;
        }

        /// <summary>
        /// 多个OrderBy用逗号隔开,属性前面带-号表示反序排序，exp:"name,-createtime"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderby"></param>
        /// <param name="condition">三方条件，true则排序有效</param>
        /// <returns></returns>
        public static IEnumerable<T> OrderByBatch<T>(this IEnumerable<T> query, string orderby, bool condition = true)
        {
            if (!condition) return query;

            var index = 0;
            var a = orderby.ToMap(',', ' ', false, false, true);
            foreach (var item in a)
            {
                var m = index++ > 0 ? "ThenBy" : "OrderBy";
                if (item.Value.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    m += "Descending";
                    orderby = item.Key;
                }
                else
                {
                    orderby = item.Key;
                }
                orderby = orderby.Trim();

                var propInfo = GetPropertyInfo(typeof(T), orderby);
                var expr = GetOrderExpression(typeof(T), propInfo);
                var method = typeof(Enumerable).GetMethods().FirstOrDefault(mt => mt.Name == m && mt.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                query = (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
            }
            return query;
        }

        /// <summary>
        /// 多个OrderBy用逗号隔开，exp:"name asc,createtime desc"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="orderby">exp:"name asc,createtime desc"</param>
        /// <param name="condition">三方条件，true则排序有效</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByBatch<T>(this IQueryable<T> query, string orderby, bool condition = true)
        {
            if (!condition) return query;

            var index = 0;
            var a = orderby.ToMap(',', ' ', false, false, true);
            foreach (var item in a)
            {
                var m = index++ > 0 ? "ThenBy" : "OrderBy";
                if (item.Value.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    m += "Descending";
                    orderby = item.Key;
                }
                else
                {
                    orderby = item.Key;
                }
                orderby = orderby.Trim();

                var propInfo = GetPropertyInfo(typeof(T), orderby);
                var expr = GetOrderExpression(typeof(T), propInfo);
                var method = typeof(Queryable).GetMethods().FirstOrDefault(mt => mt.Name == m && mt.GetParameters().Length == 2);
                var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
                query = (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
            }
            return query;
        }

        /// <summary>
        /// 正序排序单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <param name="condition">三方条件，true则排序有效</param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name, bool condition = true)
        {
            if (!condition) return query;

            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }

        /// <summary>
        /// 正序排序单个（非首个）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <param name="condition">三方条件，true则排序有效</param>
        /// <returns></returns>
        public static IQueryable<T> ThenBy<T>(this IQueryable<T> query, string name, bool condition = true)
        {
            if (!condition) return query;

            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "ThenBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }

        /// <summary>
        /// 反序排序单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <param name="condition">三方条件，true则排序有效</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string name, bool condition = true)
        {
            if (!condition) return query;

            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);
            var metMethods = typeof(Queryable).GetMethods();
            var method = metMethods.FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }

        /// <summary>
        /// 反序排序单个（非首个）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <param name="condition">三方条件，true则排序有效</param>
        /// <returns></returns>
        public static IQueryable<T> ThenByDescending<T>(this IQueryable<T> query, string name, bool condition = true)
        {
            if (!condition) return query;

            var propInfo = GetPropertyInfo(typeof(T), name);
            var expr = GetOrderExpression(typeof(T), propInfo);
            var metMethods = typeof(Queryable).GetMethods();
            var method = metMethods.FirstOrDefault(m => m.Name == "ThenByDescending" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
        }
    }
}