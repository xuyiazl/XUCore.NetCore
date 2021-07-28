using Microsoft.AspNetCore.Http;
using System;
using XUCore.Ddd.Domain.Bus;
using XUCore.Net5.Template.Applaction.Common.Interfaces;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Infrastructure.Filters;
using XUCore.NetCore;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;

namespace XUCore.Net5.Template.Applaction.Common
{
    //[DynamicWebApi(Module = "v1")]
    [DynamicWebApi]
    [ApiError]
    [ApiElapsedTime]
    public class AppService : IAppService
    {
        // 中介者 总线
        public readonly IMediatorHandler bus;

        public AppService(IMediatorHandler bus)
        {
            this.bus ??= bus;
        }
    }
}
