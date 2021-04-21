using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Ddd.Domain.Events
{
    /// <summary>
    /// 事件通知抽象基类
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    public abstract class NotificationEventHandler<TNotification> : INotificationHandler<TNotification>
         where TNotification : Event
    {
        public abstract Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}
