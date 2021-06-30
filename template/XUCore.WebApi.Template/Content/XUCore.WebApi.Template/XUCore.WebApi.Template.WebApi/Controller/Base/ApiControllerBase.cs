using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Filters;
using XUCore.NetCore.MessagePack;
using XUCore.WebApi.Template.WebApi.Controller.Filters;

namespace XUCore.WebApi.Template.WebApi.Controller
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
