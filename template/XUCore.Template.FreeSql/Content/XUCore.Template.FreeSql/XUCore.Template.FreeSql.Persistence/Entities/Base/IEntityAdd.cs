using System;

namespace XUCore.Template.FreeSql.Persistence.Entities
{
    public interface IEntityAdd<TKey> where TKey : struct
    {
        long? CreatedAtUserId { get; set; }
        string CreatedAtUserName { get; set; }
        DateTime? CreatedAt { get; set; }
    }
}