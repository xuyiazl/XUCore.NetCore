using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace XUCore.Serializer
{
    /// <summary>
    /// Json辅助扩展操作
    /// </summary>
    public static class JsonExtensions
    {
        #region ToObject(将Json字符串转换为对象)

        /// <summary>
        /// 将Json字符串转换为对象
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="json">Json字符串</param>
        /// <param name="options">json配置</param>
        /// <returns></returns>
        public static T ToObject<T>(this string json, JsonSerializerSettings options = null)
        {
            return JsonHelper.ToObject<T>(json, options);
        }

        /// <summary>
        /// 将Json字符串转换为独享
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <param name="options">json配置</param>
        /// <returns></returns>
        public static object ToObject(this string json, JsonSerializerSettings options = null)
        {
            return JsonHelper.ToObject(json, options);
        }

        #endregion ToObject(将Json字符串转换为对象)

        #region ToJson(将对象转换为Json字符串)

        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <returns></returns>
        public static string ToJson(this object target)
        {
            return JsonHelper.ToJson(target);
        }
        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="isConvertToSingleQuotes">是否将双引号转换成单引号</param>
        /// <param name="camelCase">是否驼峰式命名</param>
        /// <param name="indented">是否缩进</param>
        /// <returns></returns>
        public static string ToJson(this object target, bool isConvertToSingleQuotes = false, bool camelCase = false,
            bool indented = false)
        {
            return JsonHelper.ToJson(target, isConvertToSingleQuotes, camelCase, indented);
        }

        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string ToJson(this object target, JsonSerializerSettings options)
        {
            return JsonHelper.ToJson(target, options);
        }
        #endregion ToJson(将对象转换为Json字符串)

        #region ToJObject(将Json字符串转换为Linq对象)

        /// <summary>
        /// 将Json字符串转换为Linq对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns></returns>
        public static JObject ToJObject(this string json)
        {
            return JsonHelper.ToJObject(json);
        }

        #endregion ToJObject(将Json字符串转换为Linq对象)

        #region IsJson(判断字符串是否为Json格式)

        /// <summary>
        /// 判断字符串是否为Json格式。为效率考虑，仅做了开始和结束字符的验证
        /// </summary>
        /// <param name="json">json字符串</param>
        public static bool IsJson(this string json)
        {
            return JsonHelper.IsJson(json);
        }

        #endregion IsJson(判断字符串是否为Json格式)

        #region ToJson and ToObject (将Json字符串集合转换成Json对象集合)

        /// <summary>
        /// json列表转换为对象集合(去除空记录)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="options">json配置</param>
        /// <returns></returns>
        public static IList<T> ToObjectNotNullOrEmpty<T>(this IList<string> list, JsonSerializerSettings options = null)
        {
            return JsonHelper.ToObjectNotNullOrEmpty<T>(list, options);
        }

        /// <summary>
        /// json列表 转换为 json字符串(去除空记录)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToJsonNotNullOrEmpty(this IList<string> list)
        {
            return JsonHelper.ToJsonNotNullOrEmpty(list);
        }

        /// <summary>
        /// json列表转换为对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="options">json配置</param>
        /// <returns></returns>
        public static IList<T> ToJson<T>(this IList<string> list, JsonSerializerSettings options = null)
        {
            return JsonHelper.ToJson<T>(list, options);
        }

        /// <summary>
        /// json列表 转换为 json字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="options">json配置</param>
        /// <returns></returns>
        public static string ToJson(this IList<string> list, JsonSerializerSettings options = null)
        {
            return JsonHelper.ToJson(list, options);
        }

        #endregion ToJson and ToObject (将Json字符串集合转换成Json对象集合)
    }
}