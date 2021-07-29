﻿using System;
using XUCore.NetCore;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;
using XUCore.NetCore.MessagePack;
using Sample.Mini.Applaction.Filters;
using Sample.Mini.Core;

namespace Sample.Mini.Applaction
{
    //[DynamicWebApi(Module = "v1")]
    [DynamicWebApi]
    [ApiError]
    [ApiElapsedTime]
    [MessagePackResponseContentType]
    public class AppService : IAppService
    {
        public AppService()
        {
        }
    }
}
