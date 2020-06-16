using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Configs;
using XUCore.Develops;
using XUCore.NetCore.Redis;
using XUCore.Serializer;

namespace XUCore.NetCore.Sample
{

    public class RedisServiceSample : ISample
    {
       

        public void Run()
        {
            var user = SampleData.GetUser();

            var configuration = ConfigHelper.GetJsonConfig("appsettings.json");

            IRedisService redisService = new RedisServiceProvider(configuration, null);

            string hashId = "User";

            //redisService.HashDelete(hashId, user.Id);

            //redisService.HashSet<User>(hashId, user.Id, user);

            var res = redisService.HashGet<User>(hashId, user.Id, serializer: RedisSerializerOptions.Json);

            string hashId2 = "User2";

            //redisService.HashDelete(hashId2, "2");

            //redisService.HashSet(hashId2, "2", "2");

            var res1 = redisService.HashGet<string>(hashId2, "2", serializer: RedisSerializerOptions.Json);

            string hashId3 = "User3";

            //redisService.HashDelete(hashId3, "3");

            //redisService.HashSet(hashId3, "3", "3");

            var res3 = redisService.HashGet<string>(hashId3, "3", serializer: RedisSerializerOptions.Json);

        }
    }
}
