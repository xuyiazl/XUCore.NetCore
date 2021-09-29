using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUCore.NetCore.Formatter
{
    public class FormatterOptions
    {
        public JsonSerializerSettings JsonSerializerSettings { get; set; } = new JsonSerializerSettings()
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public IFormatterResolver FormatterResolver { get; set; } = ContractlessStandardResolver.Instance;

        public MessagePackSerializerOptions Options { get; set; } = ContractlessStandardResolver.Options;

        //public HashSet<string> SupportedContentTypes { get; set; } = new HashSet<string> { "application/json", "application/x-msgpack", "application/x-msgpack-jackson" };

        public HashSet<string> SupportedExtensions { get; set; } = new HashSet<string> { "mp" };

        public bool SuppressReadBuffering { get; set; } = false;

        public IDictionary<string, IMessagePackResponseFormatter> SupportedResponseFormatters { get; set; } = new Dictionary<string, IMessagePackResponseFormatter> {
            { "application/json" , new JsonResponseFormatter() },
            { "application/x-msgpack" , new MsgPackResponseFormatter() },
            { "application/x-msgpack-jackson" , new JacksonResponseFormatter() }
        };
    }
}
