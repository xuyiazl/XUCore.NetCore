using MediatR;
using XUCore.Ddd.Domain.Events;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Ddd.Domain.Core.Entities.User;

namespace XUCore.Template.Ddd.Domain.User.LoginRecord
{
    public class LoginRecordCreateEvent : Event
    {
        public string Id { get; set; }
        public UserLoginRecordEntity Record { get; set; }
        public LoginRecordCreateEvent(string id, UserLoginRecordEntity record)
        {
            Id = id;
            Record = record;
            AggregateId = id;
        }
        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<LoginRecordCreateEvent>
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
            public override async Task Handle(LoginRecordCreateEvent notification, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;

            }
        }
    }
}
