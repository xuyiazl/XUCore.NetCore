using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace XUCore.NetCore.Data
{
    /// <summary>
    /// 基于db上下文拓展工厂，用于拓展XUCore.NetCore.Data.BulkExtensions的GitHub开源项目
    /// </summary>
    public abstract class DBContextBase : DbContext, IDbContext
    {
        protected DBContextBase(DbContextOptions options) : base(options) { }

        public virtual DatabaseFacade Database => base.Database;

        public virtual string ConnectionStrings { get; set; }
    }
}
