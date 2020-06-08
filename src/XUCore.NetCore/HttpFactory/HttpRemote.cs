using XUCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.HttpFactory
{
    /// <summary>
    /// Http远程操作
    /// </summary>
    public static class HttpRemote
    {
        /// <summary>
        /// 获取HttpMessageService
        /// </summary>
        public static IHttpService Service
        {
            get
            {
                var httpService = Web.GetService<IHttpService>();

                if (httpService == null)
                    throw new ArgumentNullException($"请注入{nameof(IHttpService)}服务，services.HttpService(clientname, [servser])。");

                return httpService;
            }
        }
    }
}
