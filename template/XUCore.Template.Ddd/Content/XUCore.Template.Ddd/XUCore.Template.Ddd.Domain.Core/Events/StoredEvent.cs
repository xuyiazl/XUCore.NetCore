using XUCore.Template.Ddd.Domain.Core.Entities;
using System;
using XUCore.Ddd.Domain.Events;

namespace XUCore.Template.Ddd.Domain.Core.Events
{
    /// <summary>
    /// 事件存储模型
    /// </summary>
    public class StoredEvent : BaseKeyEntity
    {
        // 为了EFCore能正确CodeFirst
        public StoredEvent() { }
        /// <summary>
        /// 构造方式实例化
        /// </summary>
        /// <param name="theEvent"></param>
        /// <param name="data"></param>
        /// <param name="userId"></param>
        public StoredEvent(Event theEvent, string data, string userId)
        {
            //Id = XUCore.Helpers.Id.SequentialString.String;
            AggregateId = theEvent.AggregateId;
            MessageType = theEvent.MessageType;
            Data = data;
            UserId = userId;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 消息类型（命令）
        /// </summary>
        public string MessageType { get; protected set; }
        /// <summary>
        /// 聚合根
        /// </summary>
        public string AggregateId { get; protected set; }
        /// <summary>
        /// 存储的数据
        /// </summary>
        public string Data { get; private set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; private set; }
        /// <summary>
        /// 当前触发时间
        /// </summary>
        public DateTime Timestamp { get; private set; }
    }
}
