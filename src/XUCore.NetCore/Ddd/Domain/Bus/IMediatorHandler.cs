using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 消息总线
    /// </summary>
    public interface IMediatorHandler
    {
        /// <summary>
        /// IMediator
        /// </summary>
        IMediator Mediator { get; set; }
        /// <summary>
        /// 发布事件通知
        /// </summary>
        /// <typeparam name="TNotification"></typeparam>
        /// <param name="event">通知事件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task PublishEvent<TNotification>(TNotification @event, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : Event;
        /// <summary>
        /// 发送命令请求
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="command">命令</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TResponse> SendCommand<TResponse>(Command<TResponse> command, CancellationToken cancellationToken = default(CancellationToken));
    }
}
