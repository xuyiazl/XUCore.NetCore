using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Develops;
using XUCore.Serializer;

namespace XUCore.NetCore.Sample
{
    public class MessagePackSample : ISample
    {
        public void Run()
        {

            var user = SampleData.GetUser();

            var jj = user.ToMessagePackJson();

            var _dict = user.ToMessagePackBytes();

            var _dict1 = _dict.ToMessagePackObject<User>();

            var _user1 = user.ToMessagePackBytes().ToMessagePackObject<User>();

            var json = user.ToMessagePackJson(MessagePackSerializerResolver.UnixDateTimeOptions);

            var jsonBytes = json.ToMessagePackBytesFromJson(MessagePackSerializerResolver.UnixDateTimeOptions);

            var data = jsonBytes.ToMessagePackObject<User>(MessagePackSerializerResolver.UnixDateTimeOptions);

            //var dict = new Dictionary<string, object>();
            //dict.Add("Id", 1);
            //dict.Add("Name", "张三");
        }
    }
}
