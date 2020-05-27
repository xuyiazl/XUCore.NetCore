﻿using XUCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace XUCore.Extensions
{
    /// <summary>
    /// 系统扩展 - 类型转换
    /// </summary>
    public static partial class Extensions
    {
        #region SafeString(安全转换为字符串)

        /// <summary>
        /// 安全转换为字符串，去除两端空格，当值为null时返回""
        /// </summary>
        /// <param name="input">输入值</param>
        public static string SafeString(this object input) => input == null ? string.Empty : input.ToString().Trim();

        #endregion SafeString(安全转换为字符串)

        #region ToBool(转换为bool)

        /// <summary>
        /// 转换为bool
        /// </summary>
        /// <param name="obj">数据</param>
        public static bool ToBool(this string obj) => Conv.ToBool(obj);

        /// <summary>
        /// 转换为可空bool
        /// </summary>
        /// <param name="obj">数据</param>
        public static bool? ToBoolOrNull(this string obj) => Conv.ToBoolOrNull(obj);

        #endregion ToBool(转换为bool)

        #region ToInt(转换为int)

        /// <summary>
        /// 转换为int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int ToInt(this string obj) => Conv.ToInt(obj);

        /// <summary>
        /// 转换为可空int
        /// </summary>
        /// <param name="obj">数据</param>
        public static int? ToIntOrNull(this string obj) => Conv.ToIntOrNull(obj);

        #endregion ToInt(转换为int)

        #region ToShort(转换为Short)

        /// <summary>
        /// 转换为short
        /// </summary>
        /// <param name="obj">数据</param>
        public static short ToShort(this string obj) => Conv.ToShort(obj);

        /// <summary>
        /// 转换为可空short
        /// </summary>
        /// <param name="obj">数据</param>
        public static short? ToShortOrNull(this string obj) => Conv.ToShortOrNull(obj);

        #endregion ToShort(转换为Short)

        #region ToLong(转换为long)

        /// <summary>
        /// 转换为long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long ToLong(this string obj) => Conv.ToLong(obj);

        /// <summary>
        /// 转换为可空long
        /// </summary>
        /// <param name="obj">数据</param>
        public static long? ToLongOrNull(this string obj) => Conv.ToLongOrNull(obj);

        #endregion ToLong(转换为long)

        #region ToDouble(转换为double)

        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double ToDouble(this string obj) => Conv.ToDouble(obj);

        /// <summary>
        /// 转换为可空double
        /// </summary>
        /// <param name="obj">数据</param>
        public static double? ToDoubleOrNull(this string obj) => Conv.ToDoubleOrNull(obj);

        #endregion ToDouble(转换为double)

        #region ToDecimal(转换为decimal)

        /// <summary>
        /// 转换为decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal ToDecimal(this string obj) => Conv.ToDecimal(obj);

        /// <summary>
        /// 转换为可空decimal
        /// </summary>
        /// <param name="obj">数据</param>
        public static decimal? ToDecimalOrNull(this string obj) => Conv.ToDecimalOrNull(obj);

        #endregion ToDecimal(转换为decimal)

        #region ToDate(转换为日期)

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime ToDate(this string obj) => Conv.ToDate(obj);

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="obj">数据</param>
        public static DateTime? ToDateOrNull(this string obj) => Conv.ToDateOrNull(obj);

        #endregion ToDate(转换为日期)

        #region ToGuid(转换为Guid)

        /// <summary>
        /// 转化为Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid ToGuid(this string obj) => Conv.ToGuid(obj);

        /// <summary>
        /// 转换为可空Guid
        /// </summary>
        /// <param name="obj">数据</param>
        public static Guid? ToGuidOrNull(this string obj) => Conv.ToGuidOrNull(obj);

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="obj">数据，范例："83B0233C-A24F-49FD-8083-1337209EBC9A,EAB523C6-2FE7-47BE-89D5-C6D440C3033A"</param>
        public static List<Guid> ToGuidList(this string obj) => Conv.ToGuidList(obj);

        /// <summary>
        /// 转换为Guid集合
        /// </summary>
        /// <param name="obj">字符串集合</param>
        public static List<Guid> ToGuidList(this IList<string> obj) => obj == null ? new List<Guid>() : obj.Select(t => t.ToGuid()).ToList();

        #endregion ToGuid(转换为Guid)

        #region ToSnakeCase(将字符串转换为蛇形策略)

        /// <summary>
        /// 将字符串转换为蛇形策略
        /// </summary>
        /// <param name="str">字符串</param>
        public static string ToSnakeCase(this string str) => XUCore.Helpers.Str.ToSnakeCase(str);

        #endregion ToSnakeCase(将字符串转换为蛇形策略)

        #region ToCamelCase(将字符串转换为骆驼策略)

        /// <summary>
        /// 将字符串转换为骆驼策略
        /// </summary>
        /// <param name="str">字符串</param>
        public static string ToCamelCase(this string str) => XUCore.Helpers.Str.ToCamelCase(str);

        #endregion ToCamelCase(将字符串转换为骆驼策略)
    }
}