using MessagePack;
using MessagePack.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace XUCore.NetCore.MessagePack
{
    public class MessagePackFormatterOptions
    {
        public JsonSerializerSettings JsonSerializerSettings { get; set; } = new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            ContractResolver = new DefaultContractResolver()
        };

        public IFormatterResolver FormatterResolver { get; set; } = ContractlessStandardResolver.Instance;

        public MessagePackSerializerOptions Options { get; set; } = ContractlessStandardResolver.Options;

        public HashSet<string> SupportedContentTypes { get; set; } = new HashSet<string> { "application/json", "application/x-msgpack", "application/x-msgpack-jackson" };

        public HashSet<string> SupportedExtensions { get; set; } = new HashSet<string> { "mp" };

        public bool SuppressReadBuffering { get; set; } = false;
    }
}
