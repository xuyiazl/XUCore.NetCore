namespace XUCore.Connections
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/12/25 23:54:22
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;

    public static class Utils
    {
        public static List<EndPoint> ToIpEndPoints(IEnumerable<string> hosts)
        {
            if (hosts == null) return new List<EndPoint>();

            const int hostOrIpAddressIndex = 0;
            const int portIndex = 1;

            var ipEndpoints = new List<EndPoint>();
            foreach (var host in hosts)
            {
                var hostParts = host.Split(':');
                if (hostParts.Length == 0)
                    throw new ArgumentException("'{0}' is not a valid Host or IP Address: e.g. '127.0.0.0:11211'");

                var port = int.Parse(hostParts[portIndex]);

                var endpoint = new EndPoint(hostParts[hostOrIpAddressIndex], port);
                ipEndpoints.Add(endpoint);
            }
            return ipEndpoints;
        }
    }
}