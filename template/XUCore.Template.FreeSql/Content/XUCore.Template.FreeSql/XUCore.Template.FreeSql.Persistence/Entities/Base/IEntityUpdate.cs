using System;


namespace XUCore.Template.FreeSql.Persistence.Entities
{
    public interface IEntityUpdate<TKey> where TKey : struct
    {
        long? ModifiedAtUserId { get; set; }
        string ModifiedAtUserName { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}