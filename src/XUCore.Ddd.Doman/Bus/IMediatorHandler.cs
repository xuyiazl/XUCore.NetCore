using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Doman.Commands;
using XUCore.Ddd.Doman.Events;

namespace XUCore.Ddd.Doman.Bus
{
    public interface IMediatorHandler
    {
        Task PublishEvent<TNotification>(TNotification @event, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : Event;

        Task<TResponse> SendCommand<TResponse>(Command<TResponse> command, CancellationToken cancellationToken = default(CancellationToken));
    }
}
