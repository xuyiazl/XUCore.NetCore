using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Filters;
using XUCore.NetCore.MessagePack;

namespace XUCore.NetCore.MessageApiTest
{
    /// <summary>
    /// WebApi控制器基类
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    [ApiTrace(Ignore = true)] //忽略API请求业务跟踪
    [ApiElapsedTime]
    [MessagePackRequestContentType("application/json")]
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

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual Result<T> Success<T>(string subCode, string message, T data = default) =>
            new Result<T>()
            {
                Code = 0,
                SubCode = subCode,
                Message = message,
                Data = data,
                ElapsedTime = -1
            };

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected Result<T> Fail<T>(string subCode, string message, T data = default) =>
             new Result<T>()
             {
                 Code = 0,
                 SubCode = subCode,
                 Message = message,
                 Data = data,
                 ElapsedTime = -1
             };
    }
}