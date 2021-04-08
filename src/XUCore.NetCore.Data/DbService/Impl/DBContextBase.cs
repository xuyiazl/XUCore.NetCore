using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
    public abstract class DBContextBase : DbContext
    {
        protected DBContextBase(DbContextOptions options) : base(options) { }

        public virtual DatabaseFacade Database
        {
            get { return base.Database; }
        }

        /// <summary>
        /// <para>获取或设置一个值，该值指示是否创建事务由<see cref="DbContext"/>自动。</para>
        /// <para><see cref="Microsoft.EntityFrameworkCore.DbContext.SaveChanges()"/>如果没有调用'BeginTransaction'或'UseTransaction'方法的。将此值设置为false也会禁用<see cref="IExecutionStrategy"/></para>
        /// <para>对于<see cref="Microsoft.EntityFrameworkCore.DbContext.SaveChanges()"/>默认值为true，这意味着<see cref="Microsoft.EntityFrameworkCore.DbContext.SaveChanges()"/>将始终使用事务在保存更改。</para>
        /// <para>将此值设置为false应该非常小心，因为数据库如果<see cref="Microsoft.EntityFrameworkCore.DbContext.SaveChanges()"/>失败，可能会处于损坏状态。</para>
        /// </summary>
        public virtual bool AutoTransactionsEnabled
        {
            get
            {
                return base.Database.AutoTransactionsEnabled;
            }
            set
            {
                base.Database.AutoTransactionsEnabled = value;
            }
        }
        public virtual int SaveChanges()
        {
            return base.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public virtual IExecutionStrategy CreateExecutionStrategy()
        {
            return base.Database.CreateExecutionStrategy();
        }

        public virtual IDbContextTransaction BeginTransaction()
        {
            return base.Database.BeginTransaction();
        }

        public virtual async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await base.Database.BeginTransactionAsync(cancellationToken);
        }

        public virtual IDbContextTransaction UseTransaction(IDbContextTransaction contextTransaction)
        {
            return base.Database.UseTransaction(contextTransaction.GetDbTransaction());
        }

        public virtual async Task<IDbContextTransaction> UseTransactionAsync(IDbContextTransaction contextTransaction, CancellationToken cancellationToken = default)
        {
            return await base.Database.UseTransactionAsync(contextTransaction.GetDbTransaction(), cancellationToken);
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
