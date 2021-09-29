using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using MessagePack;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading;
using Microsoft.AspNetCore.Http;
using XUCore.Extensions;
using System.Text;
using Newtonsoft.Json;

namespace XUCore.NetCore.Formatter
{
    internal class InputFormatter : Microsoft.AspNetCore.Mvc.Formatters.InputFormatter
    {
        private readonly FormatterOptions _options;

        public InputFormatter(FormatterOptions messagePackFormatterOptions)
        {
            _options = messagePackFormatterOptions ?? throw new ArgumentNullException(nameof(messagePackFormatterOptions));
            foreach (var contentType in messagePackFormatterOptions.SupportedResponseFormatters.Keys)
            {
                SupportedMediaTypes.Add(new MediaTypeHeaderValue(contentType));
            }
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var request = context.HttpContext.Request;

            if (!request.Body.CanSeek && !_options.SuppressReadBuffering)
            {
                //使用Enablebuffering多次读取Asp Net Core 请求体
                request.EnableBuffering();

                //因为.NET Core 3.0 preview 6以后（6还是可以使用的）， Microsoft.AspNetCore.Http.Internal不再是公有方法. 所以EnableRewind不能使用。
                //BufferingHelper.EnableRewind(request);

                await request.Body.DrainAsync(CancellationToken.None);
                request.Body.Seek(0L, SeekOrigin.Begin);
            }

            if (request.ContentType.ToLower().StartsWith("application/json"))
            {
                var bytes = request.Body.ReadAllBytes();

                var json = Encoding.UTF8.GetString(bytes);

                var result = JsonConvert.DeserializeObject(json, context.ModelType, _options.JsonSerializerSettings);

                var formatterResult = await InputFormatterResult.SuccessAsync(result);

                return formatterResult;
            }
            else
            {

                var result = await MessagePackSerializer.DeserializeAsync(context.ModelType, request.Body, _options.Options);

                var formatterResult = await InputFormatterResult.SuccessAsync(result);

                return formatterResult;
            }

        }

        protected override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentException("Type cannot be null");
            }

            var typeInfo = type.GetTypeInfo();
            return !typeInfo.IsAbstract && !typeInfo.IsInterface && typeInfo.IsPublic;
        }
    }
}
