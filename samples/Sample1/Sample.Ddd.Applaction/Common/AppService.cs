using Microsoft.AspNetCore.Http;
using System;
using XUCore.Ddd.Domain.Bus;
using Sample.Ddd.Applaction.Common.Interfaces;
using Sample.Ddd.Domain.Core;
using Sample.Ddd.Infrastructure.Filters;
using XUCore.NetCore;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;
using XUCore.NetCore.MessagePack;

namespace Sample.Ddd.Applaction.Common
{
    //[DynamicWebApi(Module = "v1")]
    [DynamicWebApi]
    [ApiError]
    [ApiElapsedTime]
    [MessagePackResponseContentType]
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
