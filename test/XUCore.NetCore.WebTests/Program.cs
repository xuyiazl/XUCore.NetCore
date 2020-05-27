using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using XUCore.NetCore.Extensions;
using System.Text;

namespace XUCore.WebTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

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