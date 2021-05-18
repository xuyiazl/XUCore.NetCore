using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;

namespace XUCore.Ddd.Domain.Commands
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
        /// AutoMapper
        /// </summary>
        public readonly IMapper mapper;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandHandler()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bus">注入中介处理接口</param>
        public CommandHandler(IMediatorHandler bus)
        {
            this.bus = bus;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bus">注入中介处理接口</param>
        /// <param name="mapper">automapper</param>
        public CommandHandler(IMediatorHandler bus, IMapper mapper)
        {
            this.bus = bus;
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
