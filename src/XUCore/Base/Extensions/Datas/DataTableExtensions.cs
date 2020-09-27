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
                .Select(c => c.ColumnName.ToLower())
                .ToList();

            var properties = typeof(T).GetProperties();

            return dataTable.AsEnumerable().Select(row =>
            {
                T objT = new T();

                foreach (var property in properties)
                {
                    if (columnNames.Contains(property.Name.ToLower()))
                    {
                        if (!property.CanWrite) continue;
                        //var value = row[property.Name] == DBNull.Value ? null : row[property.Name];
                        //pro.SetValue(objT, value, null);

                        var setter = property.GetSetMethod(true);
                        if (setter != null)
                        {
                            var value = row[property.Name] == DBNull.Value ? null : row[property.Name];
                            setter.Invoke(objT, new[] { value });
                        }
                    }
                }

                return objT;

            }).ToList();
        }

        public static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            if (typeof(System.Enum).IsAssignableFrom(conversionType))
            {
                return Enum.Parse(conversionType, value.ToString());
            }
            return Convert.ChangeType(value, conversionType);
        }

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