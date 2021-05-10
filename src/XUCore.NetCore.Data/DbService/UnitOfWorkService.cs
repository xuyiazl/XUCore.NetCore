using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{
    public class UnitOfWorkService : IUnitOfWork
    {
        private readonly DbContext dbContext;
        private readonly IDbContext _db;
        public UnitOfWorkService(IDbContext context)
        {
            this._db = context;
            this.dbContext = context as DbContext;
        }

        public virtual bool AutoTransactionsEnabled
        {
            get
            {
                return dbContext.Database.AutoTransactionsEnabled;
            }
            set
            {
                dbContext.Database.AutoTransactionsEnabled = value;
            }
        }

        public virtual int Commit()
        {
            return dbContext.SaveChanges();
        }

        public virtual async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual IExecutionStrategy CreateExecutionStrategy()
        {
            return dbContext.Database.CreateExecutionStrategy();
        }

        public virtual IDbContextTransaction BeginTransaction()
        {
            return dbContext.Database.BeginTransaction();
        }

        public virtual async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public virtual IDbContextTransaction UseTransaction(IDbContextTransaction contextTransaction)
        {
            return dbContext.Database.UseTransaction(contextTransaction.GetDbTransaction());
        }

        public virtual async Task<IDbContextTransaction> UseTransactionAsync(IDbContextTransaction contextTransaction, CancellationToken cancellationToken = default)
        {
            return await dbContext.Database.UseTransactionAsync(contextTransaction.GetDbTransaction(), cancellationToken);
        }

        public virtual void CommitTransaction()
        {
            dbContext.Database.CommitTransaction();
        }

        public virtual void RollbackTransaction()
        {
            dbContext.Database.RollbackTransaction();
        }

        public virtual bool CanConnect()
        {
            return dbContext.Database.CanConnect();
        }

        public void Dispose()
        {

        }

        public virtual IDbContextTransaction CurrentTransaction => dbContext.Database.CurrentTransaction;

        public virtual IDbConnection DbConnection => dbContext.Database.GetDbConnection();
    }
}
