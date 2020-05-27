using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbRepositories
{
    public interface IDbSaveRepository<TEntity> where TEntity : class
    {
        DbContext Context { get; }
        DbSet<TEntity> Table { get; }
        DatabaseFacade Database { get; }
        bool IsNoTracking { get; set; }

        bool Save();

        Task<bool> SaveAsync();

        Task<bool> SaveAsync(CancellationToken cancellationToken = default);

        IDbContextTransaction BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}