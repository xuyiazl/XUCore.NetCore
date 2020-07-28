using Microsoft.Extensions.Logging;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.Serializer;
using XUCore.Webs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.HttpFactory
{
    /// <summary>
    /// HttpRequestMessage服务类
    /// </summary>
    public class HttpService : IHttpService
    {
        public IHttpClientFactory HttpClientFactory { get; set; }

        public HttpService(IHttpClientFactory HttpClientFactory)
        {
            this.HttpClientFactory = HttpClientFactory;
        }

        public HttpClient CreateClient(string clientName = "")
        {
            HttpClient client = HttpClientFactory.CreateClient(string.IsNullOrEmpty(clientName) ? "apiClient" : clientName);

            return client;
        }
    }
}
