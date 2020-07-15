using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.DataTest.DbRepository;
using XUCore.NetCore.DataTest.Entities;
using Microsoft.Extensions.DependencyInjection;
using XUCore.NetCore.DataTest.DbService;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace XUCore.NetCore.DataTest.Business
{

    public class AdminUsersBusinessService : IAdminUsersBusinessService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IAdminUsersDbServiceProvider adminUsersDbServiceProvider;
        private readonly INigelDbRepository<AdminUsersEntity> nigelDb;
        public AdminUsersBusinessService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.adminUsersDbServiceProvider = serviceProvider.GetService<IAdminUsersDbServiceProvider>();
            this.nigelDb = serviceProvider.GetService<INigelDbRepository<AdminUsersEntity>>();
        }

        public async Task TestAsync()
        {
            var all = await nigelDb.GetListAsync();

            var list = new List<AdminUsersEntity>();

            for (var ndx = 0; ndx < 10; ndx++)
            {
                var user = new AdminUsersEntity
                {
                    Company = "test",
                    CreatedTime = DateTime.Now,
                    Location = "test",
                    LoginCount = 0,
                    LoginLastIp = "127.0.0.1",
                    LoginLastTime = DateTime.Now,
                    Mobile = "17710146178",
                    Name = $"徐毅{ndx}",
                    Password = "123456",
                    Picture = $"徐毅{ndx}",
                    Position = $"徐毅{ ndx }",
                    Status = true,
                    UserName = "xuyi"
                };
                list.Add(user);
            }

            var res1 = await adminUsersDbServiceProvider.InsertAsync(list.ToArray());

            var res2 = adminUsersDbServiceProvider.BatchUpdate(c => c.Id > 22, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监", Company = "大牛逼公司" });

            var res3 = await adminUsersDbServiceProvider.BatchUpdateAsync(c => c.Id > 22, c => new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监", Company = "大牛逼公司" });

            var res4 = await adminUsersDbServiceProvider.BatchDeleteAsync(c => c.Id > 22);

        }
    }
}
