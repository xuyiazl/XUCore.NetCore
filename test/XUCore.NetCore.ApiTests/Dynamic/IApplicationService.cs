using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XUCore.NetCore.DynamicWebApi;

namespace XUCore.NetCore.ApiTests.Dynamic
{
    [DynamicWebApi]
    public interface IApplicationService : IDynamicWebApi
    {

    }
}
