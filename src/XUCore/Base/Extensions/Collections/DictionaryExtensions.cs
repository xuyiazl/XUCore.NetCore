using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using XUCore.Helpers;

// ReSharper disable once CheckNamespace
namespace XUCore.Extensions
{
    /// <summary>
    /// 字典(<see cref="IDictionary{TKey,TValue}"/>) 扩展
    /// </summary>
    public static class DictionaryExtensions
    {
        #region GetOrDefault(获取指定Key对应的Value，若未找到则返回默认值)

        /// <summary>
        /// 获取指定Key对应的Value，若未找到则返回默认值
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, object> dictionary, TKey key,
            TValue defaultValue = default(TValue))
        {
            return dictionary.TryGetValue(key, out var obj) ? Conv.To<TValue>(obj) : defaultValue;
        }

        /// <summary>
        /// 获取指定Key对应的Value，若未找到则返回默认值
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            TValue defaultValue = default(TValue))
        {
            return dictionary.TryGetValue(key, out var obj) ? obj : defaultValue;
        }

        #endregion GetOrDefault(获取指定Key对应的Value，若未找到则返回默认值)

        #region AddRange(批量添加键值对到字典)

        /// <summary>
        /// 批量添加键值对到字典中
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dict">字典</param>
        /// <param name="values">键值对集合</param>
        /// <param name="replaceExisted">是否替换已存在的键值对</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict,
            IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            foreach (var item in values)
            {
                if (dict.ContainsKey(item.Key) && replaceExisted)
                {
                    dict[item.Key] = item.Value;
                    continue;
                }
                if (!dict.ContainsKey(item.Key))
                {
                    dict.Add(item.Key, item.Value);
                }
            }
            return dict;
        }

        #endregion AddRange(批量添加键值对到字典)

        #region GetOrAdd(获取指定键的值，不存在则按指定委托添加值)

        /// <summary>
        /// 获取指定键的值，不存在则按指定委托添加值
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dict">字典</param>
        /// <param name="key">键</param>
        /// <param name="setValue">添加值的委托</param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
            Func<TKey, TValue> setValue)
        {
            if (!dict.TryGetValue(key, out var value))
            {
                value = setValue(key);
                dict.Add(key, value);
            }
            return value;
        }

        /// <summary>
        /// 获取指定键的值，不存在则按指定委托添加值
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">键</param>
        /// <param name="addFunc">添加值的委托</param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
            Func<TValue> addFunc)
        {
            if (dictionary.TryGetValue(key, out TValue value))
            {
                return value;
            }

            return dictionary[key] = addFunc();
        }

        #endregion GetOrAdd(获取指定键的值，不存在则按指定委托添加值)

        #region AddOrUpdate(合并两个字典)
        /// <summary>
        /// 合并两个字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic">字典</param>
        /// <param name="newDic">新字典</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dic, Dictionary<TKey, TValue> newDic)
        {
            foreach (var key in newDic.Keys)
            {
                if (dic.ContainsKey(key))
                    dic[key] = newDic[key];
                else
                    dic.Add(key, newDic[key]);
            }

            return dic;
        }

        #endregion GetOrAdd(获取指定键的值，不存在则按指定委托添加值)

        #region Sort(字段排序)

        /// <summary>
        /// 对指定的字典进行排序
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <returns>排序后的字典</returns>
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            return new SortedDictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// 对指定的字典进行排序
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="comparer">比较器，用于排序字典</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Sort<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            IComparer<TKey> comparer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }
            return new SortedDictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// 对指定的字典进行排序，根据值元素进行排序
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> SortByValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return new SortedDictionary<TKey, TValue>(dictionary).OrderBy(x => x.Value)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        #endregion Sort(字段排序)

        #region ToQueryString(将字典转换成查询字符串)

        /// <summary>
        /// 将字典转换成查询字符串
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <returns></returns>
        public static string ToQueryString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null || !dictionary.Any())
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in dictionary)
            {
                sb.Append($"{item.Key.ToString()}={item.Value.ToString()}&");
            }

            sb.TrimEnd("&");
            return sb.ToString();
        }

        #endregion ToQueryString(将字典转换成查询字符串)

        #region ToMapString(将字典转换成字符串)
        /// <summary>
        /// 将字典转换成字符串
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <param name="keyValuePairDelimiter"></param>
        /// <param name="keyValueDelimeter"></param>
        /// <returns></returns>
        public static string ToMapString<TKey, TValue>(this IDictionary<TKey, TValue> keyValuePairs, char keyValuePairDelimiter, char keyValueDelimeter)
        {
            var list = new List<string>();

            foreach (var item in keyValuePairs)
            {
                list.Add($"{item.Key}{keyValueDelimeter}{item.Value}");
            }

            return list.Join(keyValuePairDelimiter);
        }

        #endregion ToQueryString(将字典转换成查询字符串)

        #region GetKey(根据Value反向查找Key)

        /// <summary>
        /// 根据Value反向查找Key
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static TKey GetKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            foreach (var item in dictionary.Where(x => x.Value.Equals(value)))
            {
                return item.Key;
            }

            return default(TKey);
        }

        #endregion GetKey(根据Value反向查找Key)

        #region TryAdd(尝试添加键值对到字典)

        /// <summary>
        /// 尝试将键值对添加到字典中。如果不存在，则添加；存在，不添加也不抛异常
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        #endregion TryAdd(尝试添加键值对到字典)

        #region ToHashTable(将字典转换成哈希表)

        /// <summary>
        /// 将字典转换成哈希表
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <returns></returns>
        public static Hashtable ToHashTable<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var table = new Hashtable();
            foreach (var item in dictionary)
            {
                table.Add(item.Key, item.Value);
            }

            return table;
        }

        #endregion ToHashTable(将字典转换成哈希表)

        #region Invert(字典颠倒)

        /// <summary>
        /// 对指定字典进行颠倒键值对，创建新字典（值为键，键为值）
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <returns></returns>
        public static IDictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return dictionary.ToDictionary(x => x.Value, x => x.Key);
        }

        #endregion Invert(字典颠倒)

        #region AsReadOnly(转换成只读字典)

        /// <summary>
        /// 转换成只读字典
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <returns></returns>
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary)
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        #endregion AsReadOnly(转换成只读字典)

        #region EqualsTo(判断两个字典中的元素是否相等)

        /// <summary>
        /// 判断两个字典中的元素是否相等
        /// </summary>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="sourceDict">源字典</param>
        /// <param name="targetDict">目标字典</param>
        /// <exception cref="ArgumentNullException">源字典对象为空、目标字典对象为空</exception>
        public static bool EqualsTo<TKey, TValue>(this IDictionary<TKey, TValue> sourceDict,
            IDictionary<TKey, TValue> targetDict)
        {
            if (sourceDict == null)
                throw new ArgumentNullException(nameof(sourceDict), $@"源字典对象不可为空！");
            if (targetDict == null)
                throw new ArgumentNullException(nameof(sourceDict), $@"目标字典对象不可为空！");
            // 长度对比
            if (sourceDict.Count != targetDict.Count)
                return false;
            if (!sourceDict.Any() && !targetDict.Any())
                return true;
            // 深度对比
            var sourceKeyValues = sourceDict.OrderBy(x => x.Key).ToArray();
            var targetKeyValues = targetDict.OrderBy(x => x.Key).ToArray();
            var sourceKeys = sourceKeyValues.Select(x => x.Key);
            var targetKeys = targetKeyValues.Select(x => x.Key);
            var sourceValues = sourceKeyValues.Select(x => x.Value);
            var targetValues = targetKeyValues.Select(x => x.Value);
            if (sourceKeys.EqualsTo(targetKeys) && sourceValues.EqualsTo(targetValues))
                return true;
            return false;
        }

        #endregion EqualsTo(判断两个字典中的元素是否相等)

        #region KV结构数据类型映射成数据模型对象

        /// <summary>
        /// 主要运用于orm框架中查询数据对象，灵活处理按需映射为主。
        /// 亦可用于完整的KV结构转换为数据对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceDict"></param>
        /// <returns></returns>
        public static T ToModel<T>(this IDictionary<string, object> sourceDict) where T : class, new()
        {
            //获得反射查找公共属性
            T t = new T();
            var modelProperties = t.GetType().GetProperties();

            foreach (var property in modelProperties)
            {
                try
                {
                    var dict = sourceDict.FirstOrDefault(t => t.Key.Equals(property.Name));
                    //空对象继续
                    if (string.IsNullOrEmpty(dict.Value?.ToString()))
                        continue;
                    if (property.PropertyType.IsEnum)
                    {
                        //枚举转换
                        property.SetValue(t, System.Enum.Parse(property.PropertyType, dict.Value.ToString()));
                    }
                    else if (property.PropertyType.IsValueType)
                    {
                        //值类型转换
                        switch (property.PropertyType.Name.ToLower())
                        {
                            //guid特殊处理
                            case "guid":
                                property.SetValue(t, Guid.Parse(dict.Value?.ToString()));
                                break;
                            default:
                                property.SetValue(t, Convert.ChangeType(dict.Value, property.PropertyType));
                                break;
                        }
                    }
                    else
                    {
                        //引用类型
                        property.SetValue(t, dict.Value);

                    }

                }
                catch (Exception ex)
                {
                    throw new Exception($"{DateTime.Now}, {t.GetType().Name} PropertyName={property.Name} PropertyType= {property.PropertyType.Name}, Type={property.GetType().DeclaringType} Convert Error, Exception:{ex.Message}");
                }
            }
            return t;
        }

        /// <summary>
        /// 交换模型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="call"></param>
        /// <returns></returns>
        public static TModel ExChange<TKey, TValue, TModel>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue, TModel> call)
        {
            if (source == null || source.Count == 0)
                return default;

            if (source.ContainsKey(key))
                return call.Invoke(key, source[key]);

            return default;
        }
        /// <summary>
        /// 交换模型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keys"></param>
        /// <param name="call"></param>
        /// <returns></returns>
        public static IList<TModel> ExChange<TKey, TValue, TModel>(this IDictionary<TKey, TValue> source, IList<TKey> keys, Func<TKey, TValue, TModel> call)
        {
            var items = new List<TModel>();

            if (source == null || source.Count == 0)
                return null;

            if (keys == null || keys.Count == 0)
                return items;

            keys.ForEach(key =>
            {
                if (source.ContainsKey(key))
                    items.Add(call.Invoke(key, source[key]));
            });

            return items;
        }
        /// <summary>
        /// 交换模型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static KeyValue<TKey, TValue> ExChange<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            if (source == null || source.Count == 0)
                return default;

            if (source.ContainsKey(key))
                return new KeyValue<TKey, TValue>(key, source[key]);

            return default;
        }
        /// <summary>
        /// 交换模型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key"></param>
        /// <param name="split">拆分字符串，默认英文逗号</param>
        /// <returns></returns>
        public static IList<KeyValue<TKey, TValue>> ExChange<TKey, TValue>(this IDictionary<TKey, TValue> source, string key, string split = ",")
        {
            var strKeys = key.Split(split, true);

            if (strKeys == null || strKeys.Length == 0)
                return new List<KeyValue<TKey, TValue>>();

            var keys = Array.ConvertAll(strKeys, input => Conv.To<TKey>(input));

            return ExChange<TKey, TValue>(source, keys);
        }
        /// <summary>
        /// 交换模型
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static IList<KeyValue<TKey, TValue>> ExChange<TKey, TValue>(this IDictionary<TKey, TValue> source, IList<TKey> keys)
        {
            var items = new List<KeyValue<TKey, TValue>>();

            if (source == null || source.Count == 0)
                return null;

            if (keys == null || keys.Count == 0)
                return items;

            keys.ForEach(key =>
            {
                if (source.ContainsKey(key))
                    items.Add(new KeyValue<TKey, TValue>(key, source[key]));
            });

            return items;
        }


        #endregion
    }
}