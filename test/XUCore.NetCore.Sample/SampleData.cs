using MessagePack;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using XUCore.Extensions;

namespace XUCore.NetCore.Sample
{
    [MessagePackObject]
    [Serializable]
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
    [Serializable]
    public class Info
    {
        [Key(0)]
        public string Picture { get; set; }
    }

    public class SampleData
    {
        public static User GetUser()
        {
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

            return user;
        }
        public static List<User> GetUsers(int limit = 10000)
        {
            var users = new List<User>();

            limit.Times((ndx) =>
            {
                users.Add(SampleData.GetUser());
            });

            return users;
        }
    }
}
