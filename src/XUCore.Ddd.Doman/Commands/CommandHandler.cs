using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Doman.Bus;

namespace XUCore.Ddd.Doman.Commands
{
    /// <summary>
    /// 领域命令处理程序
    /// 用来作为全部处理程序的基类，提供公共方法和接口数据
    /// </summary>
    public abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        // 注入中介处理接口（目前用不到，在领域事件中用来发布事件）
        public readonly IMediatorHandler bus;

        public CommandHandler()
        {

        }

        public CommandHandler(IMediatorHandler bus)
        {
            this.bus = bus;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
