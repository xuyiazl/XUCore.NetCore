using MediatR;
using XUCore.Ddd.Domain.Events;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Template.Ddd.Domain.Auth.Role
{
    public class RoleDeleteEvent : Event
    {
        public string Id { get; set; }
        public RoleDeleteEvent(string id)
        {
            Id = id;
            AggregateId = id;
        }

        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<RoleDeleteEvent>
        {
            public Handler()
            {
            }
            /// <summary>
            /// 接受消息处理删除后的业务
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task Handle(RoleDeleteEvent notification, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;

            }
        }
    }
}
