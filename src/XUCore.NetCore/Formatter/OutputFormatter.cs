using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using MessagePack;
using Microsoft.Net.Http.Headers;
using System.Text;
using XUCore.Extensions;
using Newtonsoft.Json;

namespace XUCore.NetCore.Formatter
{
    internal class OutputFormatter : Microsoft.AspNetCore.Mvc.Formatters.OutputFormatter
    {
        private readonly FormatterOptions _options;

        public OutputFormatter(FormatterOptions messagePackFormatterOptions)
        {
            _options = messagePackFormatterOptions ?? throw new ArgumentNullException(nameof(messagePackFormatterOptions));
            foreach (var contentType in messagePackFormatterOptions.SupportedResponseFormatters.Keys)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(contentType));
            }
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var contentType = context.ContentType.SafeString().ToLower();

            if (_options.SupportedResponseFormatters.ContainsKey(contentType))
            {
                await _options.SupportedResponseFormatters[contentType].WriteAsync(context, _options);
            }
            else
            {
                context.HttpContext.Response.ContentType = "application/octet-stream";

                await MessagePackSerializer.SerializeAsync(context.ObjectType, context.HttpContext.Response.Body, context.Object, _options.Options);
            }
        }
    }

}
