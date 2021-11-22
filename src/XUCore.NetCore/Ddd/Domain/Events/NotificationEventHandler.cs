using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 事件通知抽象基类
    /// </summary>
    /// <typeparam name="TNotification"></typeparam>
    public abstract class NotificationEventHandler<TNotification> : INotificationHandler<TNotification>
         where TNotification : Event
    {
        /// <summary>
        /// 事件执行
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}
