using MessagePack;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.NetCore.MessagePack
{
    public interface IMessagePackResponseFormatter
    {
        Task WriteAsync(OutputFormatterWriteContext context, MessagePackFormatterOptions options);
    }

    public class JsonResponseFormatter : IMessagePackResponseFormatter
    {
        public async Task WriteAsync(OutputFormatterWriteContext context, MessagePackFormatterOptions options)
        {
            var res = JsonConvert.SerializeObject(context.Object, options.JsonSerializerSettings);

            var bytes = Encoding.UTF8.GetBytes(res);

            context.HttpContext.Response.ContentType = "application/json";

            await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }

    public class MsgPackResponseFormatter : IMessagePackResponseFormatter
    {
        public async Task WriteAsync(OutputFormatterWriteContext context, MessagePackFormatterOptions options)
        {
            context.HttpContext.Response.ContentType = "application/octet-stream";

            await MessagePackSerializer.SerializeAsync(context.ObjectType, context.HttpContext.Response.Body, context.Object, options.Options);
        }
    }

    public class JacksonResponseFormatter : IMessagePackResponseFormatter
    {
        public async Task WriteAsync(OutputFormatterWriteContext context, MessagePackFormatterOptions options)
        {
            var res = MessagePackSerializer.SerializeToJson(context.Object, options.Options);

            var bytes = Encoding.UTF8.GetBytes(res);

            context.HttpContext.Response.ContentType = "application/json";

            await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
