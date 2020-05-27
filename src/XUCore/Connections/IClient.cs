namespace XUCore.Connections
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/12/25 22:03:49
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;

    public interface IClient : IDisposable
    {
        string Host { get; set; }
        int Port { get; set; }
        int Id { get; set; }
    }
}