using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using XUCore.NetCore.Extensions;

namespace XUCore.ApiTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    //зЂВсецЪЕIP
                    .UseRealIp()
                    .UseStartup<Startup>();
                });
    }
}