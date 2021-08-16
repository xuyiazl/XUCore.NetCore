using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Filters;
using XUCore.NetCore.MessagePack;
using XUCore.Template.Layer.WebApi.Controller.Filters;

namespace XUCore.Template.Layer.WebApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiError]
    [ApiElapsedTime]
    [MessagePackResponseContentType]
    public class ApiControllerBase : ControllerBase
    {
        public ApiControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 日志
        /// </summary>
        public ILogger _logger;
    }
}
