using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Data.DbService
{
    public interface IUnitOfWork : IDisposable
    {
        IDbContext DbContext { get; }

        DbSet<T> Set<T>() where T : class;

        int Commit();

        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
