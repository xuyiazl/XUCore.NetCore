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
using XUCore.Serializer;

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

            var res = await url.GetAsync<ReturnModel<Answer>>(
                       clientHandler: client =>
                       {
                           client.SetHeader("limit-mode", "contain");
                           client.SetHeader("limit-field", "code,subCode,message,bodyMessage,title,content,ip,location,fromUser,userId,nickName,createTime");
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
        public FromUser FromUser { get; set; }
        public string IP { get; set; }
        public string Location { get; set; }
        public long CreateTime { get; set; }
        public override string ToString()
        {
            return this.ToJson(indented: true);
        }
    }

    public class FromUser
    {
        public int UserId { get; set; }
        public string NickName { get; set; }
    }
}
