using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.DataTest.Business
{
    public interface IAdminUsersBusinessService : IServiceDependency
    {
        Task TestAsync();
    }
}
