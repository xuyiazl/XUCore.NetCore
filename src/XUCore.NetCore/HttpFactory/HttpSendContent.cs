using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using XUCore.Extensions;
using XUCore.Serializer;

namespace XUCore.NetCore.HttpFactory
{
    /// <summary>
    /// 请求发送的数据操作
    /// </summary>
    public static class HttpSendContent
    {
        public static HttpContent JsonContent<TModel>(TModel model, Encoding encoding = null)
            => Create(model, encoding, HttpMediaType.Json);

        public static HttpContent MessagePackContent<TModel>(TModel model, Encoding encoding = null)
            => Create(model, encoding, HttpMediaType.MessagePack);

        public static HttpContent FormContent(IDictionary<string, string> nameValueCollection)
            => new FormUrlEncodedContent(nameValueCollection);

        public static HttpContent Create<TModel>(TModel model, Encoding encoding = null, HttpMediaType mediaType = HttpMediaType.Json)
        {
            HttpContent content;

            switch (mediaType)
            {
                case HttpMediaType.MessagePack:
                    content = new ByteArrayContent(model.ToMessagePackBytes());
                    break;
                case HttpMediaType.MessagePackJackson:
                    content = new StringContent(model.ToMessagePackJson(), encoding ?? Encoding.UTF8);
                    break;
                default:
                    content = new StringContent(model.ToJson(), encoding ?? Encoding.UTF8);
                    break;
            }

            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType.Description());

            return content;
        }
    }
}
