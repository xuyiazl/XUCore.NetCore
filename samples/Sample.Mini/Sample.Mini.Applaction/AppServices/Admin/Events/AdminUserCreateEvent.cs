using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Sample.Mini.Persistence.Entities.Sys.Admin;

namespace Sample.Mini.Applaction.Admin
{
    /// <summary>
    /// 管理员账号创建事件
    /// </summary>
    public class AdminUserCreateEvent : INotification
    {
        public long Id { get; set; }
        public AdminUserEntity User { get; set; }
        public AdminUserCreateEvent(long id, AdminUserEntity user)
        {
            Id = id;
            User = user;
        }

        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : INotificationHandler<AdminUserCreateEvent>
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
            public async Task Handle(AdminUserCreateEvent notification, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;
            }
        }
    }
}
