using MediatR;
using System;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public abstract class Event : Message, INotification
    {
        /// <summary>
        /// 当前触发时间
        /// </summary>
        public DateTime Timestamp
        {
            get;
            private set;
        }
        /// <summary>
        /// 事件基类
        /// </summary>
        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
