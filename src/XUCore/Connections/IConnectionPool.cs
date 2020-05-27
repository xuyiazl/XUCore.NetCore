namespace XUCore.Connections
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/12/25 22:06:57
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;

    /// <summary>
    /// 连接池接口，用于管理所有连接
    /// </summary>
    public interface IConnectionPool : IDisposable
    {
        /// <summary>
        /// 获取链接
        /// </summary>
        /// <returns></returns>
        T GetClient<T>();

        /// <summary>
        /// 获取只读链接
        /// </summary>
        /// <returns></returns>
        T GetReadOnlyClient<T>();
    }
}