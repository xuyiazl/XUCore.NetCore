using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Filters;
using XUCore.NetCore.Properties;

namespace XUCore.NetCore.Controllers
{
    /// <summary>
    /// WebApi控制器基类
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiTrace]
    [ApiElapsedTime]
    public abstract class ApiControllerBase : Controller
    {
        public ApiControllerBase(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 日志
        /// </summary>
        public ILogger _logger;

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="subCode">业务状态码</param>
        /// <param name="data">数据</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected virtual IActionResult Success(string subCode, string message = null, dynamic data = null)
        {
            if (message == null)
                message = R.Success;
            return new Result(StateCode.Ok, subCode, message, data);
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="subCode">业务状态码</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        protected IActionResult Fail(string subCode, string message) => new Result(StateCode.Fail, subCode, message);
    }
}