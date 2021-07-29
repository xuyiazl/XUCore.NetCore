using System;
using XUCore.NetCore;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;
using XUCore.NetCore.MessagePack;
using XUCore.SimpleApi.Template.Applaction.Filters;
using XUCore.SimpleApi.Template.Core;

namespace XUCore.SimpleApi.Template.Applaction
{
    //[DynamicWebApi(Module = "v1")]
    [DynamicWebApi]
    [ApiError]
    [ApiFluentValidationFilter]
    [ApiElapsedTime]
    [MessagePackResponseContentType]
    public class AppService : IAppService
    {
        public AppService()
        {
        }
    }
}
