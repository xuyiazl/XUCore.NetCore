using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// Mediator 请求插件启用配置
    /// </summary>
    public class RequestOptions
    {
        /// <summary>
        /// 请求日志，输出请求命令具体位置记录
        /// </summary>
        public bool Logger { get; set; } = true;
        /// <summary>
        /// 性能监控，输出记录超过500ms的请求
        /// </summary>
        public bool Performance { get; set; } = true;
        /// <summary>
        /// 自动验证，<see cref="IValidator"/>异常抛出，需要配合try catch捕获处理
        /// </summary>
        public bool Validation { get; set; } = true;
    }
}
