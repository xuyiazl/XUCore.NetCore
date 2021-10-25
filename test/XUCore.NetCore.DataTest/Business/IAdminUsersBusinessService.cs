using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.Business
{
    public interface IAdminUsersBusinessService : IScoped
    {
        Task TestQueryAsync();
        Task TestDbAsync();

        Task TestCacheRemove(int id, AdminUserEntity entity, AdminUserEntity o);
        Task TestCacheRemove(int id);

        Task<AdminUserEntity> TestCacheAdd(AdminUserEntity entity);

        Task TestAsync();
    }
}
