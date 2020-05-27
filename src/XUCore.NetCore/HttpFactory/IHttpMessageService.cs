using Microsoft.Extensions.Logging;
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
    public interface IHttpMessageService
    {
        IHttpClientFactory HttpClientFactory { get; set; }
        HttpClient CreateClient(string clientName = "");
    }
}
