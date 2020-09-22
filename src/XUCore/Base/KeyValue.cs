using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore
{
    /// <summary>
    /// key - value
    /// </summary>
    public class KeyValue : KeyValue<int, string>
    {
        /// <summary>
        /// 初始化一个<see cref="KeyValue"/>类型的实例
        /// </summary>
        public KeyValue() { }

        /// <summary>
        /// 初始化一个<see cref="KeyValue"/>类型的实例
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public KeyValue(int key, string value)
        {
            Key = key;
            Value = value;
        }
    }
    /// <summary>
    /// key - value
    /// </summary>
    public class KeyValue<TValue> : KeyValue<int, TValue>
    {
        /// <summary>
        /// 初始化一个<see cref="KeyValue"/>类型的实例
        /// </summary>
        public KeyValue() { }

        /// <summary>
        /// 初始化一个<see cref="KeyValue{T}"/>类型的实例
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public KeyValue(int key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
    /// <summary>
    /// key - value
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyValue<TKey, TValue>
    {
        /// <summary>
        /// key
        /// </summary>
        public TKey Key { get; set; }
        /// <summary>
        /// value
        /// </summary>
        public TValue Value { get; set; }
        /// <summary>
        /// 初始化一个<see cref="KeyValue{TKey,TValue}"/>类型的实例
        /// </summary>
        public KeyValue() { }
        /// <summary>
        /// 初始化一个<see cref="KeyValue{TKey,TValue}"/>类型的实例
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
