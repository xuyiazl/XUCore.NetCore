namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 抽象类Message，用来获取我们事件执行过程中的类名
    /// 然后并且添加聚合根
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// 消息类型（命令）
        /// </summary>
        public string MessageType { get; protected set; }
        /// <summary>
        /// 聚合根
        /// </summary>
        public string AggregateId { get; protected set; }
        ///// <summary>
        ///// 聚合根类型（主要配合聚合根的Id解决重复的问题）
        ///// </summary>
        //public string AggregateType { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
