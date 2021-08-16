using XUCore.Template.Ddd.Domain.Core.Entities.User;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Events;

namespace XUCore.Template.Ddd.Domain.User.User
{
    public class UserCreateEvent : Event
    {
        public string Id { get; set; }
        public UserEntity User { get; set; }
        public UserCreateEvent(string id, UserEntity user)
        {
            Id = id;
            User = user;
            AggregateId = id;
        }
        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<UserCreateEvent>
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
            public override async Task Handle(UserCreateEvent notification, CancellationToken cancellationToken)
            {

                await Task.CompletedTask;
            }
        }
    }
}
