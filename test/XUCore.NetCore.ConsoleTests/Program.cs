using MessagePack;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using XUCore.Configs;
using XUCore.Drawing;
using XUCore.Excel;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore.Redis;

namespace XUCore.ConsoleTests
{
    [MessagePackObject]
    public class User
    {
        [Key(0)]
        public string Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public Dictionary<string, object> Dict { get; set; }
        [Key(3)]
        public Info Info { get; set; }
        [Key(4)]
        public List<Info> Infos { get; set; }
        [Key(5)]
        public DateTime CreateTime { get; set; }
    }

    [MessagePackObject]
    public class Info
    {
        [Key(0)]
        public string Picture { get; set; }
    }

    public partial class CustomerEntity
    {
        public CustomerEntity()
        {

        }
        /// <summary>
        /// 关联用户id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string SalesMan { get; set; }
        /// <summary>
        /// 商家名
        /// </summary>
        public string VendorName { get; set; }
        /// <summary>
        /// 商家编号
        /// </summary>
        public long VendorNumber { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string Verify { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateMan { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            {
                using (var fileSteam = File.Open(@"C:\Users\Nigel\Downloads\66部客户店铺明细9.17-整理.xlsx", FileMode.Open))
                {
                    var excelReader = new ExcelReader(fileSteam);
                    var sheetReader = excelReader[0];

                    var rowCount = 0;
                    {
                        var list = new List<CustomerEntity>();

                        sheetReader.ReadNextInRow(1, 6, out rowCount, (index, row) =>
                        {
                            if (index == 1) return;

                            CustomerEntity customer = new CustomerEntity();
                            customer.UserId = 0;

                            customer.VendorName = row[0].SafeString();
                            customer.SalesMan = row[1].SafeString();
                            customer.VendorNumber = row[2].SafeString().ToLong();
                            customer.Source = row[3].SafeString();
                            customer.Verify = row[4].SafeString();
                            customer.UpdateMan = row[5].SafeString();

                            var any = list.Any(c => c.VendorName == customer.VendorName);

                            if (!any)
                                list.Add(customer);

                        });
                    }
                    {

                        var list = new List<CustomerEntity>();

                        for (var ndx = 1; ndx <= sheetReader.MaxRow; ndx++)
                        {
                            var row = sheetReader.Row(ndx).ToArray();

                            //CustomerEntity customer = new CustomerEntity();
                            //customer.UserId = 0;

                            //customer.VendorName = row[0].SafeString();
                            //customer.SalesMan = row[1].SafeString();
                            //customer.VendorNumber = row[2].SafeString().ToLong();
                            //customer.Source = row[3].SafeString();
                            //customer.Verify = row[4].SafeString();
                            //customer.UpdateMan = row[5].SafeString();

                            //var any = list.Any(c => c.VendorName == customer.VendorName);

                            //if (!any)
                            //    list.Add(customer);
                        }
                    }
                }
            }

            Console.WriteLine("done");
            Console.Read();
            {
                var file = @"C:\Users\Nigel\Downloads\1.jpg";

                ImageHelper.ZoomImage(file, @"C:\Users\Nigel\Downloads\1-1.jpg", minRatio: 30, maxSize: 100);

                var source = ImageHelper.FromFile(file);

                ImageHelper.ZoomImage(source, @"C:\Users\Nigel\Downloads\1-3.jpg", 40);

                ImageHelper.MakeThumbnail(file, @"C:\Users\Nigel\Downloads\1-2.jpg", 1000);
            }

            User user = new User
            {
                Id = "1",
                Name = "张三1111",
                CreateTime = DateTime.Now
            };
            user.Dict = new Dictionary<string, object>();
            user.Dict.Add("Id", 1);
            user.Dict.Add("Name", "张三");
            user.Info = new Info { Picture = "picture" };
            user.Infos = new List<Info> {
                    new Info { Picture = "info1" },
                    new Info { Picture = "info2" },
                    new Info { Picture = "info3" },
                    new Info { Picture = "info4" },
                    new Info { Picture = "info5" }
                };

            {


                var configuration = ConfigHelper.GetJsonConfig("appsettings.Test.json");

                IRedisService redisService = new RedisServiceProvider(configuration, null);

                //string hashId = "User";

                //redisService.HashDelete(hashId, user.Id);

                //redisService.HashSet<User>(hashId, user.Id, user);

                //var res = redisService.HashGet<User>(hashId, user.Id);

                string hashId2 = "User2";

                redisService.HashDelete(hashId2, "2");

                redisService.HashSet(hashId2, "2", "2");

                var res1 = redisService.HashGet<string>(hashId2, "2");

                string hashId3 = "User3";

                redisService.HashDelete(hashId3, "3");

                redisService.HashSet(hashId3, "3", "3");

                var res3 = redisService.HashGet<string>(hashId3, "3");


                Console.Read();



                redisService.StringSet<long>("test", 10);


                var stackConnects = configuration.GetSection<List<StackExchangeConnectionSettings>>("StackExchangeConnectionSettings");

                var endPoints = stackConnects.Where(c => c.ConnectType == ConnectTypeEnum.Read).ForEach(item => new RedLockEndPoint()
                {
                    EndPoint = new DnsEndPoint(item.EndPoint, item.Port.ToInt()),
                    RedisDatabase = item.DefaultDb,
                    Password = item.Password
                });

                IDistributedLockFactory _distributedLockFactory = RedLockFactory.Create(endPoints);

                string key = "key";
                string token = Environment.MachineName;

                Parallel.For(0, 100, async ndx =>
                    {
                        // resource 锁定的对象
                        // expiryTime 锁定过期时间，锁区域内的逻辑执行如果超过过期时间，锁将被释放
                        // waitTime 等待时间,相同的 resource 如果当前的锁被其他线程占用,最多等待时间
                        // retryTime 等待时间内，多久尝试获取一次
                        using (var redLock = await _distributedLockFactory.CreateLockAsync(
                                        resource: key,
                                        expiryTime: TimeSpan.FromSeconds(5),
                                        waitTime: TimeSpan.FromSeconds(1),
                                        retryTime: TimeSpan.FromMilliseconds(20)))
                        {
                            if (redLock.IsAcquired)
                            {
                                var count = redisService.StringGet<long>("test");

                                if (count > 0)
                                    await redisService.StringIncrementAsync("test", -1);

                                Console.WriteLine($"{count}：{DateTime.Now} {Thread.ThreadId}");
                            }
                            else
                            {
                                Console.WriteLine($"获取锁失败：{DateTime.Now} {Thread.ThreadId}");
                            }
                        }

                        //using (await _asyncLock.LockAsync())
                        //{
                        //    var s = await redisService.StringIncrementAsync("test");
                        //    Console.WriteLine($"{s}");
                        //}

                        //if (!await redisService.LockTakeAsync(key, token, 10))
                        //{
                        //    return;
                        //}

                        //try
                        //{
                        //    // 模拟执行的逻辑代码花费的时间
                        //    await Task.Delay(new Random().Next(100, 500);
                        //    if (stockCount > 0)
                        //    {
                        //        stockCount--;
                        //    }
                        //    //var s = await redisService.StringIncrementAsync("test");
                        //    Console.WriteLine($"{stockCount}");
                        //}
                        //catch (Exception ex)
                        //{
                        //}
                        //finally
                        //{
                        //    await redisService.LockReleaseAsync(key, token);
                        //}
                    });

            }
            Console.ReadKey();
        }

        public static async Task RedisLock(IRedisService redisService, string key, Action action, int lockTimeoutSeconds = 10, int replyCount = 20)
        {

            int lockCounter = 0;
            Exception logException = null;

            var lockToken = Id.Guid;
            var lockName = key + "_lock";

            while (lockCounter < replyCount)
            {
                if (!await redisService.LockTakeAsync(lockName, lockToken, lockTimeoutSeconds))
                {
                    lockCounter++;
                    //System.Threading.Thread.Sleep(50);
                    continue;
                }

                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    logException = ex;
                }
                finally
                {
                    await redisService.LockReleaseAsync(lockName, lockToken);
                }
                break;
            }

            if (lockCounter >= replyCount || logException != null)
            {
                //log it
            }
        }
    }
}
