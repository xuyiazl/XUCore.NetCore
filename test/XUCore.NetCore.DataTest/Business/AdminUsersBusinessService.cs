using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using XUCore.Extensions;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Data;
using XUCore.NetCore.DataTest.DbRepository;
using XUCore.NetCore.DataTest.DbService;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.Business
{
    public class AdminUsersBusinessService : IAdminUsersBusinessService
    {
        private readonly IAdminUsersDbServiceProvider db;
        private readonly INigelDbRepository<AdminUserEntity> nigelDb;
        private readonly INigelCopyDbRepository<AdminUserEntity> nigelCopyDb;
        private readonly INigelDbContextRepository rep;
        public AdminUsersBusinessService(IServiceProvider serviceProvider)
        {
            this.db = serviceProvider.GetService<IAdminUsersDbServiceProvider>();
            this.nigelDb = serviceProvider.GetService<INigelDbRepository<AdminUserEntity>>();
            this.nigelCopyDb = serviceProvider.GetService<INigelCopyDbRepository<AdminUserEntity>>();
            this.rep = serviceProvider.GetService<INigelDbContextRepository>();
        }

        public async Task TestQueryAsync()
        {
            await Task.CompletedTask;

            //await TestDbAsync();

            var page = new SqlPage(rep)
            {
                Fields = "a.Id",
                Table = "AdminUser a",
                Where = "a.Id = @Id",
                Orderby = "a.Id desc"
            };

            var p = page.GetPage<AdminUserEntity>(1, 10, new { Id = 1 });

            var res1 = rep.SqlFirst<AdminUserEntity>("select * from AdminUser where Id = 2");
            var res2 = rep.SqlFirst<string>("select top 1 Name from AdminUser where Id = 2");
            var res3 = rep.SqlQuery<string>("select Name from AdminUser");
            var res4 = rep.SqlQueries("select * from AdminUser where Id = @Id", new { Id = 2 });
            var res5 = rep.SqlScalar<int>("select count(*) from AdminUser where Id = @Id", new { Id = 2 });
            var res6 = rep.SqlNonQuery("update AdminUser set Name = @Name where Id = @Id", new { Name = "王五", Id = 2 });
            var res7 = rep.SqlFirst<AdminUserEntity>("select * from AdminUser where Id = 2");
        }

        public async Task TestDbAsync()
        {
            await Task.CompletedTask;

            //rep.IsAutoCommit = false;

            var list = rep.Context.User.Include(c => c.AdminUserAddress).ToList();

            var entity = BuildRecords(1)[0];

            var l = new List<AdminUserAddressEntity> { new AdminUserAddressEntity
            {
                UserId = entity.Id,
                Address = "address1",
            }, new AdminUserAddressEntity
            {
                UserId = entity.Id,
                Address = "address2",
            }, new AdminUserAddressEntity
            {
                UserId = entity.Id,
                Address = "address3",
            }, new AdminUserAddressEntity
            {
                UserId = entity.Id,
                Address = "address4",
            }, new AdminUserAddressEntity
            {
                UserId = entity.Id,
                Address = "address5",
            } };

            entity.AdminUserAddress.AddRange(l);

            rep.Add(entity);

            //var res = rep.UnitOfWork.Commit();

            var ss = rep.Context.User.Include(c => c.AdminUserAddress.Take(2)).FirstOrDefault(c => c.Id == entity.Id);
        }

        [AspectCacheRemove(HashKey = "Cache_Test", Key = "{Id}_{Name}_{UserName}_{0}")]
        public async Task TestCacheRemove(int id, AdminUserEntity entity, AdminUserEntity o)
        {
            await Task.CompletedTask;
        }

        [AspectCacheRemove(HashKey = "mytest", Key = "{0}")]
        public async Task TestCacheRemove(int id)
        {
            await Task.CompletedTask;
        }

        [AspectCache(HashKey = "mytest", Key = "{Id}")]
        public async Task<AdminUserEntity> TestCacheAdd(AdminUserEntity entity)
        {
            await Task.CompletedTask;

            rep.Delete<AdminUserEntity>(c => c.Id == 0);
            return entity;
        }

        public async Task TestAsync()
        {
            {
                //此例子证明 多数据库的分布式事务在Core 3.1以及以下版本是不支持的，官方表示在Net5中支持分布式事务
                //see https://github.com/dotnet/runtime/issues/715
                nigelDb.UnitOfWork.CreateTransactionScope(
                    run: (tran) =>
                    {
                        nigelDb.Delete(c => true);
                        //nigelCopyDb.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));

                        //nigelCopyDb.Add(BuildRecords(10));

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
                nigelDb.UnitOfWork.AutoTransactionsEnabled = false;
                //创建事务
                using (var tran = nigelDb.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        nigelDb.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));
                        //当更新的时候 Loaction超过长度出现异常，进入事务回滚
                        nigelDb.Update(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }
            {
                // 禁用SaveChanges默认的事务提交
                db.Write.UnitOfWork.AutoTransactionsEnabled = false;

                using (var tran = db.Write.UnitOfWork.BeginTransaction())
                {
                    try
                    {
                        db.Delete(c => true);

                        db.Add(BuildRecords(10));

                        db.Update(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                    }
                }
            }
            {
                nigelDb.UnitOfWork.CreateTransaction(
                    (tran) =>
                    {
                        nigelDb.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));

                        nigelDb.Update(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });
                    },
                    (tran, error) =>
                    {
                        Console.WriteLine(error.FormatMessage());
                    });
            }
            {
                var res = nigelDb.UnitOfWork.CreateTransaction(
                    (tran) =>
                    {
                        nigelDb.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));

                        nigelDb.Update(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });

                        return true;
                    },
                    (tran, error) =>
                    {
                        Console.WriteLine(error.FormatMessage());

                        return false;
                    });
            }
            {
                await nigelDb.UnitOfWork.CreateTransactionAsync(
                    async (tran, cancel) =>
                    {
                        await nigelDb.DeleteAsync(c => true);

                        await nigelDb.AddAsync(BuildRecords(10));

                        await nigelDb.UpdateAsync(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });
                    },
                    async (tran, error, cancel) =>
                    {
                        await Task.CompletedTask;

                        Console.WriteLine(error.FormatMessage());
                    },
                    CancellationToken.None);
            }
            {
                var res = await nigelDb.UnitOfWork.CreateTransactionAsync(
                    async (tran, cancel) =>
                    {
                        await nigelDb.DeleteAsync(c => true);

                        await nigelDb.AddAsync(BuildRecords(10));

                        await nigelDb.UpdateAsync(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });

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
                var strategy = nigelDb.UnitOfWork.CreateExecutionStrategy();

                strategy.Execute(() =>
                {
                    using (var tran = nigelDb.UnitOfWork.BeginTransaction())
                    {
                        try
                        {
                            nigelDb.Delete(c => true);

                            nigelDb.Add(BuildRecords(10));

                            nigelDb.Update(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });

                            tran.Commit();
                        }
                        catch
                        {
                            tran.Rollback();
                        }
                    }
                });
            }
            {
                var strategy = db.Write.UnitOfWork.CreateExecutionStrategy();

                strategy.Execute(() =>
                {
                    using (var tran = db.Write.UnitOfWork.BeginTransaction())
                    {
                        try
                        {
                            db.Delete(c => true);

                            db.Add(BuildRecords(10));

                            db.Update(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });

                            tran.Commit();
                        }
                        catch
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
                nigelDb.UnitOfWork.CreateTransactionScope(
                    run: (tran) =>
                    {
                        db.Delete(c => true);

                        nigelDb.Add(BuildRecords(10));

                        db.Update(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });
                    },
                    error: (tran, error) =>
                    {

                    });
            }
            {
                var res = nigelDb.UnitOfWork.CreateTransactionScope(
                     run: (tran) =>
                     {
                         db.Delete(c => true);

                         nigelDb.Add(BuildRecords(10));

                         db.Update(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });

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

                        db.Update(c => c.Id > 5, new AdminUserEntity() { Name = "哈德斯" });

                        tran.Complete();
                    }
                    catch
                    {

                    }
                }
            }

            {
                //var all = await nigelDb.GetListAsync();

                var list = BuildRecords(10);

                await db.AddAsync(list.ToArray());

                var res2 = db.Update(c => c.Id > 22, new AdminUserEntity() { Name = "哈德斯" });

                var res3 = await db.UpdateAsync(c => c.Id > 22, c => new AdminUserEntity() { Name = "哈德斯" });

                var res4 = await db.DeleteAsync(c => c.Id > 22);
            }
        }

        public IList<AdminUserEntity> BuildRecords(int limit)
        {
            var list = new List<AdminUserEntity>();

            limit.Times(ndx =>
            {
                var user = new AdminUserEntity
                {
                    Created_At = DateTime.Now,
                    Deleted_At = DateTime.Now,
                    Updated_At = DateTime.Now,
                    Name = $"徐毅{ndx}",
                    Password = "123456",
                    Picture = $"徐毅{ndx}",
                    Status = 1,
                    UserName = "xuyi"
                };
                5.Times(a =>
                {
                    user.AdminUserAddress.Add(new AdminUserAddressEntity
                    {
                        Address = $"address{a}",
                    });
                });
                list.Add(user);
            });

            return list;
        }
    }
}
