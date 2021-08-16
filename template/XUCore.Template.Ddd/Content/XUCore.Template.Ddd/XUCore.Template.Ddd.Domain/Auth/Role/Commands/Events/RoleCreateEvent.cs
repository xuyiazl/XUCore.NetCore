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
    public class RoleCreateEvent : Event
    {
        public string Id { get; set; }
        public RoleEntity Role { get; set; }
        public RoleCreateEvent(string id, RoleEntity role)
        {
            Id = id;
            Role = role;
            AggregateId = id;
        }

        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<RoleCreateEvent>
        {
            public Handler()
            {
            }
            /// <summary>
            /// 接受消息处理创建后的业务
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task Handle(RoleCreateEvent notification, CancellationToken cancellationToken)
            {

                await Task.CompletedTask;
            }
        }
    }
}
