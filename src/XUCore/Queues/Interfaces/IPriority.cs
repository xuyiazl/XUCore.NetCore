namespace XUCore.Queues
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.1
    *           Created by 徐毅 at 2011/1/14 10:25:03
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    /// <summary>
    /// 优先级接口
    /// </summary>
    public interface IPriority
    {
        /// <summary>
        /// 优先级枚举
        /// </summary>
        PriorityEnums Priority { get; set; }
    }
}