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

            var jj = user.ToMsgPackJson();

            var _dict = user.ToMsgPackBytes();

            var _dict1 = _dict.ToMsgPackObject<User>();

            var _user1 = user.ToMsgPackBytes().ToMsgPackObject<User>();

            var json = user.ToMsgPackJson(MessagePackSerializerResolver.UnixDateTimeOptions);

            var jsonBytes = json.ToMsgPackBytesFromJson(MessagePackSerializerResolver.UnixDateTimeOptions);

            var data = jsonBytes.ToMsgPackObject<User>(MessagePackSerializerResolver.UnixDateTimeOptions);

            //var dict = new Dictionary<string, object>();
            //dict.Add("Id", 1);
            //dict.Add("Name", "张三");
        }
    }
}
