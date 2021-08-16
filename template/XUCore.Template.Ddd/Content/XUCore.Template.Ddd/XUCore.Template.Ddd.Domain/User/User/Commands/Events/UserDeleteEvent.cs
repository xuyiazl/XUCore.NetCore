using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Events;

namespace XUCore.Template.Ddd.Domain.User.User
{
    public class UserDeleteEvent : Event
    {
        public string Id { get; set; }
        public UserDeleteEvent(string id)
        {
            Id = id;
            AggregateId = id;
        }
        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<UserDeleteEvent>
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
            public override async Task Handle(UserDeleteEvent notification, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;

            }
        }
    }
}
