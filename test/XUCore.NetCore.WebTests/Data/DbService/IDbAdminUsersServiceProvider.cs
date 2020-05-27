using XUCore.Paging;
using XUCore.WebTests.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.DbService
{
    public interface IDbAdminUsersServiceProvider : IDbDependencyService
    {
        int Delete(AdminUsers entity);
        Task<AdminUsers> GetByIdAsync(long id);
        Task<int> GetCountAsync(Expression<Func<AdminUsers, bool>> selector);
        Task<List<AdminUsers>> GetListAsync(Expression<Func<AdminUsers, bool>> expression, string orderby, int limit);
        Task<PagedSkipModel<AdminUsers>> GetPagedSkipListAsync(Expression<Func<AdminUsers, bool>> selector, string orderby, int skip = 0, int limit = 20);
        Task<PagedModel<AdminUsers>> GetPagedListAsync(Expression<Func<AdminUsers, bool>> expression, string orderby, int pageNumber = 1, int pageSize = 20);
        int Insert(AdminUsers entity);
        int Insert(AdminUsers[] entity);
        int Update(AdminUsers entity);
        Task<int> BatchUpdateAsync(Expression<Func<AdminUsers, bool>> selector, Expression<Func<AdminUsers, AdminUsers>> update, CancellationToken cancellationToken = default);
        Task<int> BatchDeleteAsync(Expression<Func<AdminUsers, bool>> selector, CancellationToken cancellationToken = default);
    }
}