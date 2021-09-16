using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace XUCore.NetCore.FreeSql.Entity
{
    public interface IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        TKey Id { get; set; }
    }

    public interface IEntity : IEntity<long>
    {
    }

    public class Entity<TKey> : IEntity<TKey> where TKey : struct
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Description("主键Id")]
        [Column(Position = 1, IsIdentity = true, IsPrimary = true)]
        public virtual TKey Id { get; set; }
    }

    public class Entity : Entity<long>
    {
    }
}