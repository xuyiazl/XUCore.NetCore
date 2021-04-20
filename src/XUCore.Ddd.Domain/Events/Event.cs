using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Ddd.Domain.Events
{
    /// <summary>
    /// 事件基类
    /// </summary>
    public abstract class Event : Message<bool>, INotification
    {
        /// <summary>
        /// 当前触发时间
        /// </summary>
        public DateTime Timestamp
        {
            get;
            private set;
        }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
