using XUCore.IdGenerators.Abstractions;
using XUCore.IdGenerators.Ids;

namespace XUCore.IdGenerators.Core
{
    /// <summary>
    /// 雪花算法ID 生成器
    /// </summary>
    public class SnowflakeIdGenerator : ILongGenerator
    {
        /// <summary>
        /// 雪花算法ID
        /// </summary>
        private readonly SnowflakeId _id = new SnowflakeId(1, 1);

        /// <summary>
        /// 获取<see cref="SnowflakeIdGenerator"/>类型的实例
        /// </summary>
        public static SnowflakeIdGenerator Current { get; } = new SnowflakeIdGenerator();

        /// <summary>
        /// 创建ID
        /// </summary>
        /// <returns></returns>
        public long Create()
        {
            return _id.NextId();
        }
    }
}