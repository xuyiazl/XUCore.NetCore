using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
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
using System.Text.RegularExpressions;

namespace XUCore.NetCore.Sample
{
    public class MyTestService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;
        public MyTestService(ILogger<MyTestService> logger, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            {
                var url =
                    UrlArguments.Create("server", "api/Answer/GetByEnt")
                        .Add("themeId", 200004);

                var responseMessage = await HttpRemote.Service.CreateClient("test")
                    .SetHeaderAccept(HttpMediaType.Json)
                    .PostAsync(url, new List<string>() { "0001" });

                if (!responseMessage.IsSuccessStatusCode)
                {
                    Console.WriteLine(responseMessage.StatusCode);
                }
                else
                {
                    var content = await responseMessage.Content.ReadAsMessagePackAsync<ReturnModel<Answer>>();

                    Console.WriteLine(content);
                }
            }
            {
                var httpOptions = serviceProvider.GetService<IReturnHttpOptions<ReturnModel<Answer>>>();

                var res = await
                    UrlArguments.Create("server", "api/Answer/GetByEnt")
                        .Add("themeId", 200004)
                        .GetAsync(httpOptions, cancellationToken: cancellationToken);
            }
            {
                var httpOptions = serviceProvider.GetService<IReturnHttpOptions<ReturnModel<Answer>>>();

                httpOptions.ClientHandler = client =>
                {
                    client.SetHeader("limit-mode", "contain");
                    client.SetHeader("limit-field", "code,subCode,message,bodyMessage,title,content,ip,location,fromUser,userId,nickName,createTime");
                    client.SetHeader("limit-field-rename", "themeTitle=title");
                    //client.SetHeader("limit-date-format", "yyyy-MM-dd'T'HH:mm:ss'Z'");
                    //client.SetHeader("limit-date-unix", "true");
                };

                var res = await
                    UrlArguments.Create("server", "api/Answer/GetByEnt")
                        .Add("themeId", 200004)
                        .GetAsync(httpOptions, cancellationToken: cancellationToken);
            }
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
        public DateTime CreateTime { get; set; }
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
