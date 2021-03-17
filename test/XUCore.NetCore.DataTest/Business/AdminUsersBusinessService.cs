using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using XUCore.Extensions;
using XUCore.NetCore.Data.DbService;
using XUCore.NetCore.DataTest.DbRepository;
using XUCore.NetCore.DataTest.DbService;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.Business
{

    public class AdminUsersBusinessService : IAdminUsersBusinessService
    {
        private readonly IAdminUsersDbServiceProvider db;
        private readonly INigelReadDbRepository<AdminUsersEntity> nigelDb;
        private readonly INigelCopyDbRepository<AdminUsersEntity> nigelCopyDb;
        public AdminUsersBusinessService(IServiceProvider serviceProvider)
        {
            this.db = serviceProvider.GetService<IAdminUsersDbServiceProvider>();
            this.nigelDb = serviceProvider.GetService<INigelReadDbRepository<AdminUsersEntity>>();
            this.nigelCopyDb = serviceProvider.GetService<INigelCopyDbRepository<AdminUsersEntity>>();
        }

        public async Task TestAsync()
        {
            {
                //此例子证明 多数据库的分布式事务在Core 3.1以及以下版本是不支持的，官方表示在Net5中支持分布式事务
                //see https://github.com/dotnet/runtime/issues/715
                nigelDb.DbContext.CreateTransactionScope(
                    run: (tran) =>
                    {
                        nigelDb.Delete(c => true);
                        nigelCopyDb.Delete(c => true);
                        
                        nigelDb.Add(BuildRecords(10));

                        nigelCopyDb.Add(BuildRecords(10));

                        //nigelDb.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });
                    },
                    error: (tran, error) =>
                    {

                    });
            }
            //
            //
            //  https://www.cnblogs.com/yaopengfei/p/11387935.html 具体参照
            //
            //

            // 默认事务(SaveChanges)

            // 我们在使用SaveChanges的时候默认是使用事务的。
            // 通过nigelDb.DbContext.AutoTransactionsEnabled = false; 进行关闭

            //
            // DbContextTransaction事务
            //
            /*
                该事务为EF6新增的事务，通常用于手动接管事务，某些操作是一个事务，某些操作是另外一个事务。

　　            使用场景：EF调用SQL语句的时候使用该事务、 多个SaveChanges的情况(解决自增ID另一个业务中使用的场景)。

　　            核心代码：BeginTransaction、Commit、Rollback、Dispose. 如果放到using块中,就不需要手动Dispose了。

　　            该种事务与数据库中的transaction事务原理一致,在EF中，由于每次调用 db.Database.ExecuteSqlCommand(sql1, pars1)的时候,即刻执行了该SQL语句,所以要把他放到一个大的事务中，整体提交、回滚.(EF6新增的)
             
             */
            {
                // 禁用SaveChanges默认的事务提交
                nigelDb.DbContext.AutoTransactionsEnabled = false;
                //创建事务
                using (var tran = nigelDb.DbContext.BeginTransaction())
                {
                    try
                    {
                        nigelDb.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));
                        //当更新的时候 Loaction超过长度出现异常，进入事务回滚
                        nigelDb.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }
            {
                // 禁用SaveChanges默认的事务提交
                db.Write.DbContext.AutoTransactionsEnabled = false;

                using (var tran = db.Write.DbContext.BeginTransaction())
                {
                    try
                    {
                        db.Delete(c => true);

                        db.Add(BuildRecords(10));

                        db.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                    }
                }
            }
            {
                nigelDb.DbContext.CreateTransaction(
                    (tran) =>
                    {
                        nigelDb.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));

                        nigelDb.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });
                    },
                    (tran, error) =>
                    {
                        Console.WriteLine(error.FormatMessage());
                    });
            }
            {
                var res = nigelDb.DbContext.CreateTransaction(
                    (tran) =>
                    {
                        nigelDb.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));

                        nigelDb.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });

                        return true;
                    },
                    (tran, error) =>
                    {
                        Console.WriteLine(error.FormatMessage());

                        return false;
                    });
            }
            {
                await nigelDb.DbContext.CreateTransactionAsync(
                    async (tran, cancel) =>
                    {
                        await nigelDb.DeleteAsync(c => true);

                        await nigelDb.AddAsync(BuildRecords(10));

                        await nigelDb.UpdateAsync(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });
                    },
                    async (tran, error, cancel) =>
                    {
                        await Task.CompletedTask;

                        Console.WriteLine(error.FormatMessage());
                    },
                    CancellationToken.None);
            }
            {
                var res = await nigelDb.DbContext.CreateTransactionAsync(
                    async (tran, cancel) =>
                    {
                        await nigelDb.DeleteAsync(c => true);

                        await nigelDb.AddAsync(BuildRecords(10));

                        await nigelDb.UpdateAsync(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });

                        return true;
                    },
                    async (tran, error, cancel) =>
                    {
                        await Task.CompletedTask;

                        Console.WriteLine(error.FormatMessage());

                        return false;
                    },
                    CancellationToken.None);
            }
            {
                var strategy = nigelDb.DbContext.CreateExecutionStrategy();

                strategy.Execute(() =>
                {
                    using (var tran = nigelDb.DbContext.BeginTransaction())
                    {
                        try
                        {
                            nigelDb.Delete(c => true);

                            nigelDb.Add(BuildRecords(10));

                            nigelDb.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });

                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                        }
                    }
                });
            }
            {
                var strategy = db.Write.DbContext.CreateExecutionStrategy();

                strategy.Execute(() =>
                {
                    using (var tran = db.Write.DbContext.BeginTransaction())
                    {
                        try
                        {
                            db.Delete(c => true);

                            db.Add(BuildRecords(10));

                            db.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });

                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                        }
                    }
                });
            }
            //
            //  TransactionScope事务(环境事务)
            //
            /*
                1. 该事务用来处理多个SaveChanges的事务(特殊情况的业务)或者多个DBContext(每个DBContext是一个实例,代表不同的数据库连接).

                2. 核心代码：（一个Complete函数走天下,异常的话,自动回滚 ，也可以结合try-catch Transaction.Current.Rollback();实现回滚）

　　　　　　                   需要引入程序集：using System.Transactions;

                3. 适用场景：
　　                ①该种事务适用于多数据库连接的情况
　　                特别注意：
                         如果使用该事务来处理多个数据库(多个DBContex)时,必须手动开启msdtc服务,这样才可以将多个DB的SaveChange给放到一个事务中，如果失败， 则多个数据库的数据统一回滚.
 　                      开启msdtc服务的步骤： cmd命令→net start msdtc
 　                      ②主键id自增的情况,同一个业务线中，需要拿到新增加的数据的主键id，进行操作。
                         ③多线程带锁的情况，同一条业务线前半部分必须先SaveChanges，才能保证数据准确性(测试简单版本，实际的业务场景待补充！！！)
             */
            {
                nigelDb.DbContext.CreateTransactionScope(
                    run: (tran) =>
                    {
                        db.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));

                        db.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });
                    },
                    error: (tran, error) =>
                    {

                    });
            }
            {
                var res = nigelDb.DbContext.CreateTransactionScope(
                     run: (tran) =>
                     {
                         db.Delete(c => true);

                         nigelDb.Add(BuildRecords(10));

                         db.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });

                         return true;
                     },
                     error: (tran, error) =>
                     {
                         return false;
                     });
            }
            {
                using (var tran = new TransactionScope())
                {
                    try
                    {
                        db.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));

                        db.Update(c => c.Id > 5, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监吹牛逼总监", Company = "大牛逼公司" });

                        tran.Complete();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return;

            {
                //var all = await nigelDb.GetListAsync();

                var list = BuildRecords(10);

                var res1 = await db.AddAsync(list.ToArray());

                var res2 = db.Update(c => c.Id > 22, new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监", Company = "大牛逼公司" });

                var res3 = await db.UpdateAsync(c => c.Id > 22, c => new AdminUsersEntity() { Name = "哈德斯", Location = "吹牛逼总监", Company = "大牛逼公司" });

                var res4 = await db.DeleteAsync(c => c.Id > 22);
            }
        }

        public IList<AdminUsersEntity> BuildRecords(int limit)
        {
            var list = new List<AdminUsersEntity>();

            for (var ndx = 0; ndx < limit; ndx++)
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

            return list;
        }
    }
}
