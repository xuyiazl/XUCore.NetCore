using MediatR;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.FreeSql.Persistence.Entities.User;

namespace XUCore.Template.FreeSql.DbService.Events
{
    /// <summary>
    /// 用户账号创建事件
    /// </summary>
    public class UserCreateEvent : INotification
    {
        public long Id { get; set; }
        public UserEntity User { get; set; }
        public UserCreateEvent(long id, UserEntity user)
        {
            Id = id;
            User = user;
        }

        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : INotificationHandler<UserCreateEvent>
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
            public async Task Handle(UserCreateEvent notification, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;
            }
        }
    }
}
