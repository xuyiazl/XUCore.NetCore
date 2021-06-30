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
            //���������������
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //linuxϵͳ�Ͽ�����С�����߳�
            ThreadPool.SetMinThreads(250, 250);

            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostingContext, _config) =>
               {
                   //��ʾ���õ�ǰ��������Ŀ¼
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
                          ////�Ƿ���������ͬ������
                          //options.AllowSynchronousIO = true;
                          //options.ApplicationSchedulingMode = SchedulingMode.ThreadPool;
                          //����https,��pfx֤�鶪��binĿ¼����,ͬʱ��������ļ��������봦��
                          //net �ͻ����������https��ʱ����ܻᴥ��ssl����ʧ�����⣬�ڷ��������ʱ����룺
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
