using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.Mongo.Test
{
    public class MainService : IHostedService
    {
        private readonly ILogger logger;
        private readonly IMongoService<UserMongoModel> mongoService;
        public MainService(ILogger<MainService> logger, IMongoService<UserMongoModel> mongoService)
        {
            this.logger = logger;
            this.mongoService = mongoService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await mongoService.AddAsync(new UserMongoModel { }, cancellationToken: cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
