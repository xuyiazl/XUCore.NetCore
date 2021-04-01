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
        Task TestAspectCore();

        Task TestCacheRemove();

        Task<AdminUsersEntity> TestCacheAdd();

        Task TestAsync();
    }
}
