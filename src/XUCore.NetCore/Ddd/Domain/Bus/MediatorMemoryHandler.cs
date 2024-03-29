﻿using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// Mediator 消息中介发布请求和通知（存储溯源事件）
    /// </summary>
    public class MediatorMemoryStoreHandler : IMediatorHandler
    {
        /// <summary>
        /// IMediator
        /// </summary>
        public IMediator Mediator { get; set; }

        /// <summary>
        /// 事件仓储服务
        /// </summary>
        private readonly IEventStoreService eventStoreService;

        /// <summary>
        /// 不需要存储的事件
        /// </summary>
        public IList<string> NotEventStore = new List<string>() { "DomainNotification" };

        public MediatorMemoryStoreHandler(IMediator mediator, IEventStoreService eventStoreService)
        {
            this.Mediator = mediator;
            this.eventStoreService = eventStoreService;
        }

        /// <summary>
        /// 发布事件通知
        /// </summary>
        /// <typeparam name="TNotification"></typeparam>
        /// <param name="event">通知事件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task PublishEvent<TNotification>(TNotification @event, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : Event
        {
            // 除了领域通知以外的事件都保存下来
            if (!NotEventStore.Contains(@event.MessageType))
                eventStoreService?.Save(@event);

            // MediatR中介者模式中的第二种方法，发布/订阅模式
            return Mediator.Publish(@event);
        }

        /// <summary>
        /// 发送命令请求
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="command">命令</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResponse> SendCommand<TResponse>(Command<TResponse> command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Mediator.Send(command, cancellationToken);
        }
    }

    /// <summary>
    /// Mediator 消息中介发布请求和通知
    /// </summary>
    public class MediatorMemoryHandler : IMediatorHandler
    {
        /// <summary>
        /// IMediator
        /// </summary>
        public IMediator Mediator { get; set; }

        public MediatorMemoryHandler(IMediator mediator)
        {
            this.Mediator = mediator;
        }

        /// <summary>
        /// 发布事件通知
        /// </summary>
        /// <typeparam name="TNotification"></typeparam>
        /// <param name="event">通知事件</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task PublishEvent<TNotification>(TNotification @event, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : Event
        {
            // MediatR中介者模式中的第二种方法，发布/订阅模式
            return Mediator.Publish(@event);
        }

        /// <summary>
        /// 发送命令请求
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="command">命令</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<TResponse> SendCommand<TResponse>(Command<TResponse> command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Mediator.Send(command, cancellationToken);
        }
    }
}
