using System;


namespace XUCore.NetCore.FreeSql.Entity
{
    public interface IEntityUpdate<TKey>
    {
        long? ModifiedAtUserId { get; set; }
        string ModifiedAtUserName { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}