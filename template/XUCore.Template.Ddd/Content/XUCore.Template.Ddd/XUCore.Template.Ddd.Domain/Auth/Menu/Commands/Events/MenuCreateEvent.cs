using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Events;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    public class MenuCreateEvent : Event
    {
        public string Id { get; set; }
        public MenuEntity Menu { get; set; }
        public MenuCreateEvent(string id, MenuEntity menu)
        {
            Id = id;
            Menu = menu;
            AggregateId = id;
        }

        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<MenuCreateEvent>
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
            public override async Task Handle(MenuCreateEvent notification, CancellationToken cancellationToken)
            {

                await Task.CompletedTask;
            }
        }
    }
}
