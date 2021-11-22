using MediatR;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    //public abstract class Command<TResponse> : Message, IRequest<TResponse>
    public abstract class Command<TResponse> : Command, IRequest<TResponse>
    {
        ///// <summary>
        ///// 时间戳
        ///// </summary>
        //public DateTime Timestamp { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        protected Command() : base()
        {
            //Timestamp = DateTime.Now;
        }
    }
}
