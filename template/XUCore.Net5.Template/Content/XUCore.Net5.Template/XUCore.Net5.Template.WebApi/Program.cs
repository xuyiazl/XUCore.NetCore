using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //解决中文乱码问题
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //linux系统上开启最小工作线程
            ThreadPool.SetMinThreads(250, 250);

            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, _config) =>
               {
                   //显示设置当前程序运行目录
                   _config.SetBasePath(Directory.GetCurrentDirectory());
                   //_config.AddJsonFile("hosting.json", optional: true);
                   _config.AddJsonFile("appsettings.json", optional: true);
                   _config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                   _config.AddEnvironmentVariables(prefix: "PREFIX_");
                   _config.AddCommandLine(args);
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                      //.UseRealIp("X-Real-IP")
                      .UseContentRoot(Directory.GetCurrentDirectory())
                      //.UseIISIntegration()
                      .UseKestrel(options =>
                      {
                          //options.AddServerHeader = false;
                          ////是否允许请求同步操作
                          //options.AllowSynchronousIO = true;
                          //options.ApplicationSchedulingMode = SchedulingMode.ThreadPool;
                          //配置https,将pfx证书丢到bin目录下面,同时针对配置文件进行密码处理
                          //net 客户端请求服务https的时候可能会触发ssl握手失败问题，在发起请求的时候加入：
                          //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                          options.ConfigureHttpsDefaults(s =>
                          {
                              s.SslProtocols =
                                SslProtocols.Tls |
                                SslProtocols.Tls11 |
                                SslProtocols.Tls12;
                          });
                      })

                      .ConfigureLogging((hostingContext, builder) =>
                      {
                          builder.ClearProviders();
                          builder.SetMinimumLevel(LogLevel.Trace);
                          builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                          builder.AddConsole();
                          builder.AddDebug();
                      })
                      .UseStartup<Startup>();
                })
                .UseInterceptorHostBuilder()
                ;
    }
}
