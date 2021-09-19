using MessagePack;
using System;
using System.Collections.Generic;
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

    class Program
    {
        static void Main(string[] args)
        {
            {
                {
                    using var excelReader = new ExcelReader(@"C:\Users\Nigel\Downloads\结算-出港结算明细_导出任务_20210903_11324755.xls");
                    using var sheetReader = excelReader[0];

                    sheetReader.ReadNextInRow(1, 63, out int rowCount, (index, row) =>
                    {

                    });
                }
                //{
                //    using var excelReader = new ExcelReader(@"C:\Users\Nigel\Downloads\1.xlsx");
                //    using var sheetReader = excelReader[0];

                //    for (var ndx = 1; ndx <= sheetReader.MaxRow; ndx++)
                //    {
                //        var row = sheetReader.Row(ndx).ToArray();
                //    }
                //}

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
                }
                Console.ReadKey();
            }
        }
    }
}
