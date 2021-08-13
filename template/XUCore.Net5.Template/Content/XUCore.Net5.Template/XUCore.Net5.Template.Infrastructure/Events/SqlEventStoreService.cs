using Microsoft.Extensions.DependencyInjection;
using System;
using XUCore.Ddd.Domain.Events;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Events;
using XUCore.Net5.Template.Infrastructure.Authorization;
using XUCore.Serializer;

namespace XUCore.Net5.Template.Infrastructure.Events
{
    /// <summary>
    /// 事件存储服务类
    /// </summary>
    public class SqlEventStoreService : IEventStoreService
    {
        private readonly IDefaultDbRepository db;
        private readonly IServiceProvider serviceProvider;
        public SqlEventStoreService(IServiceProvider serviceProvider, IDefaultDbRepository db)
        {
            this.db = db;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 保存事件模型统一方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theEvent"></param>
        public void Save<T>(T theEvent) where T : Event
        {
            var serializedData = theEvent.ToJson();

            var auth = serviceProvider.GetService<IAuthService>();

            var storedEvent = new StoredEvent(
                theEvent,
                serializedData,
                auth.UserId ?? "");

            db.Add(storedEvent);
        }
    }
}
