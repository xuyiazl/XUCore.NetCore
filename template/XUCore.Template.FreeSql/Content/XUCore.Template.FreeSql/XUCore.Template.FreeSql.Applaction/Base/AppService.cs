using XUCore.Ddd.Domain.Filters;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Filters;
using XUCore.NetCore.MessagePack;
using XUCore.Template.FreeSql.Applaction.Filters;

namespace XUCore.Template.FreeSql.Applaction
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
