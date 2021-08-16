using XUCore.Ddd.Domain.Bus;
using XUCore.Template.Ddd.Applaction.Common.Interfaces;
using XUCore.Template.Ddd.Infrastructure.Filters;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;
using XUCore.NetCore.MessagePack;

namespace XUCore.Template.Ddd.Applaction.Common
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
