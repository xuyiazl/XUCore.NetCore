using XUCore.Helpers;
using System.Collections.Specialized;
using System.Text;

// ReSharper disable once CheckNamespace
namespace XUCore.Extensions
{
    /// <summary>
    /// 键值对集合(<see cref="NameValueCollection"/>) 扩展
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        #region ToQueryString(将键值对集合转换成查询字符串)

        /// <summary>
        /// 将键值对集合转换成查询字符串
        /// </summary>
        /// <param name="collection">键值对集合</param>
        /// <returns></returns>
        public static string ToQueryString(this NameValueCollection collection)
        {
            if (collection == null || !collection.HasKeys())
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (string key in collection.Keys)
            {
                sb.Append($"{key}={collection[key]}&");
            }

            sb.TrimEnd("&");
            return sb.ToString();
        }

        #endregion ToQueryString(将键值对集合转换成查询字符串)

        /// <summary>
        /// Gets the value associated w/ the key, if it's empty returns the default value.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetOrDefault(NameValueCollection collection, string key, string defaultValue)
        {
            if (collection == null) return defaultValue;

            string val = collection[key];
            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return val;
        }

        /// <summary>
        /// Gets the value associated w/ the key and convert it to the correct Type, if empty returns the default value.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="collection">Collection.</param>
        /// <param name="key">The key representing the value to get.</param>
        /// <param name="defaultValue">Value to return if the key has an empty value.</param>
        /// <returns></returns>
        public static T GetOrDefault<T>(NameValueCollection collection, string key, T defaultValue)
        {
            if (collection == null) return defaultValue;

            string val = collection[key];
            if (string.IsNullOrEmpty(val))
                return defaultValue;

            return Conv.To<T>(val);
        }
    }
}