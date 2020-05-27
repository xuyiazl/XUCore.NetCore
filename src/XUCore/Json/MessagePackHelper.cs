using MessagePack;
using MessagePack.Resolvers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Json
{
    /// <summary>
    /// MessagePack操作辅助类
    /// </summary>
    public class MessagePackHelper
    {
        /// <summary>
        /// 将对象转换为MessagePack Json字符串
        /// </summary> 
        /// <param name="target">目标对象</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static string ToJson(object target, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (target == null)
                return string.Empty;

            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var json = MessagePackSerializer.SerializeToJson(target, options, cancellationToken);

            return json;
        }
        /// <summary>
        /// 将对象转换为MessagePack Bytes
        /// </summary>
        /// <param name="json">MessagePack Json字符串</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static byte[] ToBytesFromJson(string json, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var buffers = MessagePackSerializer.ConvertFromJson(json, options, cancellationToken);

            return buffers;
        }
        /// <summary>
        /// 将对象转换为MessagePack Bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static string ToJsonFromBytes(byte[] bytes, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (bytes == null || bytes.Length == 0)
                return default;

            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var buffers = MessagePackSerializer.ConvertToJson(bytes, options, cancellationToken);

            return buffers;
        }
        /// <summary>
        /// 将对象转换为MessagePack Bytes
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static byte[] ToBytes(object target, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (target == null)
                return default;

            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var buffers = MessagePackSerializer.Serialize(target, options, cancellationToken);

            return buffers;
        }
        /// <summary>
        /// 将MessagePack Bytes转换为对象
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static T ToObject<T>(byte[] bytes, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (bytes == null || bytes.Length == 0)
                return default;

            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var res = MessagePackSerializer.Deserialize<T>(bytes, options, cancellationToken);

            return res;
        }
        /// <summary>
        /// 将MessagePack Bytes转换为对象
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static T ToObject<T>(Stream stream, MessagePackSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            if (stream == null || stream.Length == 0)
                return default;

            options = options ?? MessagePackSerializerResolver.DateTimeOptions;

            var res = MessagePackSerializer.Deserialize<T>(stream, options, cancellationToken);

            return res;
        }
    }
}
