using XUCore.Helpers;
using XUCore.Serializer;
using XUCore.Webs;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.NetCore.HttpFactory
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, UrlBuilder urlBuilder, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlBuilder.Complete().Url;

            return await httpClient.GetAsync(requestUrl, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PostAsync<TModel>(this HttpClient httpClient, UrlBuilder urlBuilder, TModel model, CancellationToken cancellationToken = default)
        {
            var content = HttpSendContent.JsonContent(model, Encoding.UTF8);

            return await httpClient.PostAsync(urlBuilder, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, UrlBuilder urlBuilder, HttpContent content, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlBuilder.Complete().Url;

            return await httpClient.PostAsync(requestUrl, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PutAsync<TModel>(this HttpClient httpClient, UrlBuilder urlBuilder, TModel model, CancellationToken cancellationToken = default)
        {
            var content = HttpSendContent.JsonContent(model, Encoding.UTF8);

            return await httpClient.PutAsync(urlBuilder, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, UrlBuilder urlBuilder, HttpContent content, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlBuilder.Complete().Url;

            return await httpClient.PutAsync(requestUrl, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PatchAsync<TModel>(this HttpClient httpClient, UrlBuilder urlBuilder, TModel model, CancellationToken cancellationToken = default)
        {
            var content = HttpSendContent.JsonContent(model, Encoding.UTF8);

            return await httpClient.PatchAsync(urlBuilder, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, UrlBuilder urlBuilder, HttpContent content, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlBuilder.Complete().Url;

            return await httpClient.PatchAsync(requestUrl, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, UrlBuilder urlBuilder, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlBuilder.Complete().Url;

            return await httpClient.DeleteAsync(requestUrl, cancellationToken);
        }
    }
}
