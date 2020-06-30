using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace XUCore.Extensions.Datas
{
    /// <summary>
    /// 数据表(<see cref="DataTable"/>) 扩展
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// DataTable转换为泛型集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="dataTable">数据表</param>
        public static IList<T> ToList<T>(this DataTable dataTable) where T : new()
        {
            if (dataTable == null)
                throw new ArgumentNullException(nameof(dataTable), @"数据表不可为空！");

            var columnNames = dataTable.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var properties = typeof(T).GetProperties();

            return dataTable.AsEnumerable().Select(row =>
            {
                T objT = new T();

                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        if (!pro.CanWrite) continue;
                        var value = row[pro.Name] == DBNull.Value ? null : row[pro.Name];
                        pro.SetValue(objT, Convert.ChangeType(value, pro.PropertyType), null);
                    }
                }

                return objT;

            }).ToList();
        }
        ///// <summary>
        ///// DataTable转换为泛型集合
        ///// </summary>
        ///// <typeparam name="T">类型</typeparam>
        ///// <param name="dataTable">数据表</param>
        //public static IList<T> ToList<T>(this DataTable dataTable)
        //{
        //    if (dataTable == null)
        //        throw new ArgumentNullException(nameof(dataTable), @"数据表不可为空！");
        //    var type = typeof(T);
        //    var properties = type.GetProperties();
        //    var constructors =
        //        type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        //    var noParamCtor = constructors.Single(x => x.GetParameters().Length == 0);
        //    var collection = new List<T>();
        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        var instance = (T)noParamCtor.Invoke(null);
        //        foreach (var property in properties)
        //        {
        //            if (dataTable.Columns.Contains(property.Name))
        //            {
        //                var setter = property.GetSetMethod(true);
        //                if (setter != null)
        //                {
        //                    var value = row[property.Name] == DBNull.Value ? null : row[property.Name];
        //                    setter.Invoke(instance, new[] { value });
        //                }
        //            }
        //        }

        //        collection.Add(instance);
        //    }

        //    return collection;
        //}
    }
}