using MessagePack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Json
{
    /// <summary>
    /// MessagePack辅助扩展操作
    /// </summary>
    public static class MessagePackExtensions
    {
        /// <summary>
        /// 将对象转换为MessagePack Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static string ToMsgPackJson(this object target, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            return MessagePackHelper.ToJson(target, options, cancellationToken);
        }
        /// <summary>
        /// 将对象转换为MessagePack Bytes
        /// </summary>
        /// <param name="json">MessagePack Json字符串</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static byte[] ToMsgPackBytesFromJson(this string json, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            return MessagePackHelper.ToBytesFromJson(json, options, cancellationToken);
        }
        /// <summary>
        /// 将对象转换为MessagePack Bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static string ToMsgPackJsonFromBytes(this byte[] bytes, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            return MessagePackHelper.ToJsonFromBytes(bytes, options, cancellationToken);
        }
        /// <summary>
        /// 将对象转换为MessagePack Bytes
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static byte[] ToMsgPackBytes(this object target, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            return MessagePackHelper.ToBytes(target, options, cancellationToken);
        }
        /// <summary>
        /// 将MessagePack Bytes转换为对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static T ToMsgPackObject<T>(this byte[] bytes, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            return MessagePackHelper.ToObject<T>(bytes, options, cancellationToken);
        }
        /// <summary>
        /// 将MessagePack stream转换为对象
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static T ToMsgPackObject<T>(this Stream stream, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            return MessagePackHelper.ToObject<T>(stream, options, cancellationToken);
        }
    }
}
