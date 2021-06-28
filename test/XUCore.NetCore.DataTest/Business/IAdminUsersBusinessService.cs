using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.Business
{
    public interface IAdminUsersBusinessService : IServiceDependency
    {
        Task TestQueryAsync();
        Task TestDbAsync();
        Task TestAspectCore();

        Task TestCacheRemove(int id, AdminUserEntity entity, AdminUserEntity o);
        Task TestCacheRemove(int id);

        Task<AdminUserEntity> TestCacheAdd(AdminUserEntity entity);

        Task TestAsync();
    }
}
