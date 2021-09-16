using System;

namespace XUCore.NetCore.FreeSql.Entity
{
    public interface IEntityAdd<TKey>
    {
        long? CreatedAtUserId { get; set; }
        string CreatedAtUserName { get; set; }
        DateTime? CreatedAt { get; set; }
    }
}