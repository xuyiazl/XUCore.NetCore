using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 领域命令处理程序
    /// 用来作为全部处理程序的基类，提供公共方法和接口数据
    /// </summary>
    public abstract class CommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
         where TCommand : Command<TResponse>
    {
        /// <summary>
        /// 注入中介处理接口（目前用不到，在领域事件中用来发布事件）
        /// </summary>
        public readonly IMediatorHandler bus;
        /// <summary>
        /// IMediator
        /// </summary>
        public readonly IMediator mediator;
        /// <summary>
        /// AutoMapper
        /// </summary>
        public readonly IMapper mapper;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandHandler() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mediator">注入中介处理接口</param>
        public CommandHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mediator">注入中介处理接口</param>
        /// <param name="mapper">automapper</param>
        public CommandHandler(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bus">注入中介处理接口</param>
        public CommandHandler(IMediatorHandler bus)
        {
            this.bus = bus;
            this.mediator = bus.Mediator;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bus">注入中介处理接口</param>
        /// <param name="mapper">automapper</param>
        public CommandHandler(IMediatorHandler bus, IMapper mapper)
        {
            this.bus = bus;
            this.mediator = bus.Mediator;
            this.mapper = mapper;
        }
        /// <summary>
        /// 事件执行
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
