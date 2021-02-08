using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Develops.ShellProgressBar;
using XUCore.NetCore;
using XUCore.NetCore.Extensions;

namespace XUCore.NetCore.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    //配置根目录
                    configHost.SetBasePath(Path.GetDirectoryName(typeof(Program).Assembly.Location));
                    //读取环境变量，Asp.Net core默认的环境变量是以ASPNETCORE_作为前缀的，这里也采用此前缀以保持一致
                    configHost.AddEnvironmentVariables("ASPNETCORE_");
                    //可以在启动host的时候之前可传入参数，暂不需要先注释掉，可根据需要开启
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    //读取应用特定环境下的配置json
                    configApp.AddJsonFile($"appsettings.json", optional: true);
                    configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    //读取环境变量
                    configApp.AddEnvironmentVariables();
                    //可以在启动host的时候之前可传入参数，暂不需要先注释掉，可根据需要开启
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.ClearProviders();
                    //configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    //configLogging.AddConsole();
                    //if (hostContext.HostingEnvironment.IsEnvironment("dev") || hostContext.HostingEnvironment.IsEnvironment("test"))
                    //    configLogging.AddDebug();
                })
                .UseConsoleLifetime();

        static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            //services.AddLogging();
            services.AddHttpService("server", "https://testmsrightsapi.tostar.top");

            services.AddHostedService<MyTestService>();
        }
    }
}
