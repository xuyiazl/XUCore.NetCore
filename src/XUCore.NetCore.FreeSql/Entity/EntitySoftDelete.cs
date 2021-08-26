using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace XUCore.NetCore.FreeSql.Entity
{
    /// <summary>
    /// 实体软删除
    /// </summary>
    public class EntitySoftDelete<TKey> : Entity<TKey>, IEntitySoftDelete 
        where TKey : struct
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -1)]
        public bool IsDeleted { get; set; } = false;
    }

    public class EntitySoftDelete : EntitySoftDelete<long>
    {
    }
}