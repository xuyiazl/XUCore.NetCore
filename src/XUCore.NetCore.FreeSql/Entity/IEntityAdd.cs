using System;

namespace XUCore.NetCore.FreeSql.Entity
{
    public interface IEntityAdd<TKey> where TKey : struct
    {
        long? CreatedAtUserId { get; set; }
        string CreatedAtUserName { get; set; }
        DateTime? CreatedAt { get; set; }
    }
}