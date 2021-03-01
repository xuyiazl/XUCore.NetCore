using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Mongo.Test
{
    public static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddLogging();

            services.AddSingleton(typeof(IMongoServiceProvider<>), typeof(MongoServiceProvider<>));

            services.AddHostedService<MainService>();
        }
    }
}
