using XUCore.NetCore.Data.DbService;
using XUCore.Paging;
using XUCore.WebTests.Data.Entity;
using XUCore.WebTests.Data.Repository.ReadRepository;
using XUCore.WebTests.Data.Repository.WriteRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.DbService
{
    public class DbAdminUsersServiceProvider : DbServiceBaseProvider<AdminUsers>, IDbAdminUsersServiceProvider
    {
        public DbAdminUsersServiceProvider(IReadRepository<AdminUsers> readRepository, IWriteRepository<AdminUsers> writeRepository)
            : base(readRepository, writeRepository)
        {

        }

        public new async Task<int> GetCountAsync(Expression<Func<AdminUsers, bool>> selector)
        {
            return await readRepository.GetCountAsync(selector);
        }

        public Task<List<AdminUsers>> GetListAsync(Expression<Func<AdminUsers, bool>> selector, string orderby, int limit)
        {
            return readRepository.GetListAsync(selector, orderby, 0, limit);
        }

        public new async Task<PagedSkipModel<AdminUsers>> GetPagedSkipListAsync(Expression<Func<AdminUsers, bool>> selector, string orderby, int skip = 0, int limit = 20)
        {
            return await readRepository.GetPagedSkipListAsync(selector, orderby, skip, limit);
        }

        public new async Task<PagedModel<AdminUsers>> GetPagedListAsync(Expression<Func<AdminUsers, bool>> selector, string orderby, int pageIndex = 1, int pageSize = 20)
        {
            return await readRepository.GetPagedListAsync(selector, orderby, pageIndex, pageSize);
        }

        public async Task<AdminUsers> GetByIdAsync(long id)
        {
            return await readRepository.GetByIdAsync(id);
        }

        public new int Insert(AdminUsers entity)
        {
            return writeRepository.Insert(entity);
        }

        public new int Insert(AdminUsers[] entity)
        {
            return writeRepository.BatchInsert(entity);
        }

        public new int Update(AdminUsers entity)
        {
            return writeRepository.Update(entity);
        }

        public new async Task<int> BatchUpdateAsync(Expression<Func<AdminUsers, bool>> selector, AdminUsers updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default)
        {
            return await writeRepository.BatchUpdateAsync(selector, updateValues, updateColumns, cancellationToken);
        }
        public new int BatchUpdate(Expression<Func<AdminUsers, bool>> selector, AdminUsers updateValues, List<string> updateColumns = null)
        {
            return writeRepository.BatchUpdate(selector, updateValues, updateColumns);
        }

        public new async Task<int> BatchUpdateAsync(Expression<Func<AdminUsers, bool>> selector, Expression<Func<AdminUsers, AdminUsers>> update, CancellationToken cancellationToken = default)
        {
            return await writeRepository.BatchUpdateAsync(selector, update, cancellationToken);
        }

        public new int Delete(AdminUsers entity)
        {
            return writeRepository.Delete(entity);
        }
        public new async Task<int> BatchDeleteAsync(Expression<Func<AdminUsers, bool>> selector, CancellationToken cancellationToken = default)
        {
            return await writeRepository.BatchDeleteAsync(selector, cancellationToken);
        }

    }
}
