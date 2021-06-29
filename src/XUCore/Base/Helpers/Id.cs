using XUCore.IdGenerators.Abstractions;
using XUCore.IdGenerators.Core;
using System;

namespace XUCore.Helpers
{
    /// <summary>
    /// Id 生成器
    /// </summary>
    public static class Id
    {
        /// <summary>
        /// Id
        /// </summary>
        private static string _id;
        /// <summary>
        /// Guid 生成器
        /// </summary>
        private static IGuidGenerator GuidGenerator { get; set; } = SequentialGuidGenerator.Current;

        /// <summary>
        /// Long 生成器
        /// </summary>
        private static ILongGenerator LongGenerator { get; set; } = SnowflakeIdGenerator.Current;

        /// <summary>
        /// String 生成器
        /// </summary>
        private static IStringGenerator StringGenerator { get; set; } = TimestampIdGenerator.Current;
        /// <summary>
        /// ObjectId 生成器
        /// </summary>
        private static IStringGenerator ObjectGenerator { get; set; } = ObjectIdGenerator.Current;

        /// <summary>
        /// 设置Id
        /// </summary>
        /// <param name="id">Id</param>
        public static void SetId(string id) => _id = id;

        /// <summary>
        /// 重置Id
        /// </summary>
        public static void Reset() => _id = null;

        /// <summary>
        /// 用Guid创建标识，去掉分隔符
        /// </summary>
        public static string Guid => string.IsNullOrWhiteSpace(_id) ? System.Guid.NewGuid().ToString("N") : _id;

        /// <summary>
        /// 创建 雪花算法Id
        /// </summary>
        public static long SnowflakeId => LongGenerator.Create();

        /// <summary>
        /// 创建 时间戳Id
        /// </summary>
        public static string TimestampId => StringGenerator.Create();

        /// <summary>
        /// 创建 ObjectId
        /// </summary>
        public static string ObjectId => ObjectGenerator.Create();
        /// <summary>
        /// 有序GUid
        /// </summary>
        public static class Sequential
        {
            /// <summary>
            /// 创建 有序Guid（生成的GUID 按照二进制的顺序排列）
            /// </summary>
            public static Guid Binary => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAsBinary);
            /// <summary>
            /// 创建 有序Guid（生成的GUID 按照字符串顺序排列）
            /// </summary>
            public static Guid String => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAsString);
            /// <summary>
            /// 创建 有序Guid（生成的GUID 像SQL Server, 按照末尾部分排列）
            /// </summary>
            public static Guid End => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAtEnd);
        }
        /// <summary>
        /// 有序GUid（ToString("N")）
        /// </summary>
        public static class SequentialString
        {
            /// <summary>
            /// 创建 有序Guid（生成的GUID 按照二进制的顺序排列）
            /// </summary>
            public static string Binary => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAsBinary).ToString("N");
            /// <summary>
            /// 创建 有序Guid（生成的GUID 按照字符串顺序排列）
            /// </summary>
            public static string String => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAsString).ToString("N");
            /// <summary>
            /// 创建 有序Guid（生成的GUID 像SQL Server, 按照末尾部分排列）
            /// </summary>
            public static string End => SequentialGuidGenerator.Current.Create(SequentialGuidType.SequentialAtEnd).ToString("N");
        }
    }
}