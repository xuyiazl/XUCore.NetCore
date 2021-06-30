using XUCore.Ddd.Domain.Events;

namespace XUCore.Net5.Template.Infrastructure.Events
{
    /// <summary>
    /// 事件存储服务类
    /// </summary>
    public class SqlEventStoreService : IEventStoreService
    {
        // 注入仓储接口
        //private readonly IEventStoreRepository _eventStoreRepository;
        //private readonly IUser _user;
        public SqlEventStoreService(/*IEventStoreRepository eventStoreRepository, IUser user*/)
        {
            //_eventStoreRepository = eventStoreRepository;
            //_user = user;
        }

        /// <summary>
        /// 保存事件模型统一方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="theEvent"></param>
        public void Save<T>(T theEvent) where T : Event
        {
            // 对事件模型序列化
            //var serializedData = theEvent.ToJson();

            //var storedEvent = new StoredEvent(
            //    theEvent,
            //    serializedData,
            //    _user.Name);

            //eventStoreRepository.Store(storedEvent);
        }
    }
}
