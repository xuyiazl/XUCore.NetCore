using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Ddd.Doman.Events
{
    /// <summary>
    /// 抽象类Message，用来获取我们事件执行过程中的类名
    /// 然后并且添加聚合根
    /// </summary>
    public abstract class Message<TResponse> : IRequest<TResponse>
    {
        /// <summary>
        /// 消息类型（命令）
        /// </summary>
        public string MessageType { get; protected set; }
        /// <summary>
        /// 聚合根（也可用Guid，但是由于目前大多数的Id均为UUID所以这里也使用long）
        /// </summary>
        public long AggregateId { get; protected set; }
        /// <summary>
        /// 聚合根类型（主要配合聚合根的Id解决重复的问题）
        /// </summary>
        public string AggregateType { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
