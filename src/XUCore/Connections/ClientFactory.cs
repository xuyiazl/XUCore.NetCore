namespace XUCore.Connections
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/12/25 22:27:42
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    public class ClientFactory : IClientFactory
    {
        public virtual IClient Create(string host, int port)
        {
            return null;
        }
    }
}