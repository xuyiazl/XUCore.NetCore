using MediatR;
using XUCore.Ddd.Domain.Events;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    public class MenuUpdateEvent : Event
    {
        public string Id { get; set; }
        public MenuEntity Menu { get; set; }
        public MenuUpdateEvent(string id, MenuEntity menu)
        {
            Id = id;
            Menu = menu;
            AggregateId = id;
        }

        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<MenuUpdateEvent>
        {
            public Handler()
            {
            }
            /// <summary>
            /// 接受消息处理修改后的业务
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task Handle(MenuUpdateEvent notification, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;

            }
        }
    }
}
