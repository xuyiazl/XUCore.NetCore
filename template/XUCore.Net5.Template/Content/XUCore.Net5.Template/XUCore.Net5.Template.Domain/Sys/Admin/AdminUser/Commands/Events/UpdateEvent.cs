using MediatR;
using XUCore.Ddd.Domain.Events;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    public class UpdateEvent : Event
    {
        public long Id { get; set; }
        public AdminUserEntity AdminUser { get; set; }
        public UpdateEvent(long id, AdminUserEntity user)
        {
            Id = id;
            AdminUser = user;
            AggregateId = id.ToString();
        }
        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<UpdateEvent>
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
            public override async Task Handle(UpdateEvent notification, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;

            }
        }
    }
}
