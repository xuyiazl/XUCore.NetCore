using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.DataTest.DbRepository;
using XUCore.NetCore.Redis;

namespace XUCore.NetCore.DataTest
{
    public static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddLogging();

            services.AddNigelDbContext();
            services.AddNigelCopyDbContext();

            services.AddReadDbContext();
            services.AddWriteDbContext();

            services.AddScanLifetime();

            //添加redis插件，支持拦截器缓存
            services.AddRedisService().AddJsonRedisSerializer();

            //注册缓存服务，暂时只提供内存缓存，后面会增加Redis，需要视情况而定
            services.AddCacheInterceptor((option) =>
            {
                option.CacheMode = CacheMode.Redis;
                option.RedisRead = "cache-read";
                option.RedisWrite = "cache-write";
            });
            //services.AddInterceptor();

            services.AddHostedService<MainService>();
        }
    }
}
