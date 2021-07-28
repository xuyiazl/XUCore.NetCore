using System;
using XUCore.NetCore;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;
using Sample.Mini.Applaction.Filters;
using Sample.Mini.Core;

namespace Sample.Mini.Applaction
{
    //[DynamicWebApi(Module = "v1")]
    [DynamicWebApi]
    [ApiError]
    [ApiElapsedTime]
    public class AppService : IAppService
    {
        public AppService()
        {
        }
    }
}
