using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{
    /// <summary>
    /// 基于db上下文拓展工厂，用于拓展XUCore.NetCore.Data.BulkExtensions的GitHub开源项目
    /// </summary>
    public abstract class DBContextFactory : DbContext
    {
        /// <summary>
        /// 映射的路径
        /// </summary>
        protected string mappingPath { get; set; }
        protected DBContextFactory(DbContextOptions options, string mappingPath) : base(options)
        {
            this.mappingPath = mappingPath;
        }

        public virtual int SaveChanges()
        {
            return base.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public virtual IDbContextTransaction BeginTransaction()
        {
            return base.Database.BeginTransaction();
        }

        public virtual async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await base.Database.BeginTransactionAsync(cancellationToken);
        }

        public virtual void CommitTransaction()
        {
            base.Database.CommitTransaction();
        }

        public virtual void RollbackTransaction()
        {
            base.Database.RollbackTransaction();
        }

        public virtual bool CanConnect()
        {
            return base.Database.CanConnect();
        }

        public virtual IDbContextTransaction CurrentTransaction => base.Database.CurrentTransaction;
    }
}
