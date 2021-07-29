using XUCore.Ddd.Domain.Filters;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;
using XUCore.NetCore.MessagePack;
using Simple.Applaction.Filters;

namespace Simple.Applaction
{
    //[DynamicWebApi(Module = "v1")]
    [DynamicWebApi]
    [ApiError]
    [ApiElapsedTime]
    [CommandValidation]
    [MessagePackResponseContentType]
    public class AppService : IAppService
    {
        public AppService()
        {
        }
    }
}
