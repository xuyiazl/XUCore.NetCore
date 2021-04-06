using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.DataTest.Business;

namespace XUCore.NetCore.DataTest
{
    public class MainService : IHostedService
    {
        private readonly ILogger logger;
        private readonly IAdminUsersBusinessService adminUsersBusinessService;
        public MainService(ILogger<MainService> logger, IAdminUsersBusinessService adminUsersBusinessService)
        {
            this.logger = logger;
            this.adminUsersBusinessService = adminUsersBusinessService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await adminUsersBusinessService.TestCacheRemove(1111, new Entities.AdminUsersEntity
            {
                Id = 1,
                Name = "name",
                UserName = "username"
            },
            null);
            await adminUsersBusinessService.TestCacheAdd();
            await adminUsersBusinessService.TestCacheAdd();
            await adminUsersBusinessService.TestCacheAdd();
            await adminUsersBusinessService.TestCacheAdd();
            await adminUsersBusinessService.TestCacheAdd();
            await adminUsersBusinessService.TestCacheAdd();
            await adminUsersBusinessService.TestCacheAdd();

            //await adminUsersBusinessService.TestAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}
