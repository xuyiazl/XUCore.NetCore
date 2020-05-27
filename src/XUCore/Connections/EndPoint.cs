namespace XUCore.Connections
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/12/25 23:56:48
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    public partial class EndPoint
    {
        public EndPoint(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }

        public string Host { get; set; }
        public int Port { get; set; }
    }
}