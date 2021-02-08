using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.HttpFactory;
using XUCore.Webs;
using XUCore.Extensions;

namespace XUCore.NetCore.Sample
{
    public class MyTestService : IHostedService
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly IHttpService httpService;
        public MyTestService(ILogger<MyTestService> logger, IConfiguration configuration, IHttpService httpService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.httpService = httpService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var url = UrlArguments.Create("server", "api/Answer/GetByEnt").Add("themeId", 200004);
            
            var respKeys = await url.GetAsync<ReturnModel<Answer>>(
                       clientHandler: client =>
                       {
                           client.SetHeader("limit-mode", "contain");

                           client.SetHeader("limit-field", "code,subCode,message,bodyMessage,title,content,userId,entName,createTime");

                           client.SetHeader("limit-field-rename", "themeTitle=title");

                           //client.SetHeader("limit-date-format", "yyyy-MM-dd'T'HH:mm:ss'Z'");
                           client.SetHeader("limit-date-unix", "true");
                       }
                        , cancellationToken: cancellationToken);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }

    public class Answer
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public string EntName { get; set; }
        public long CreateTime { get; set; }
    }
}
