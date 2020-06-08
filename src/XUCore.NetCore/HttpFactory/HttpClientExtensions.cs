using XUCore.Helpers;
using XUCore.Json;
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
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, UrlArguments urlArguments, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlArguments.Complete().Url;

            return await httpClient.GetAsync(requestUrl, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PostAsync<TModel>(this HttpClient httpClient, UrlArguments urlArguments, TModel model, CancellationToken cancellationToken = default)
        {
            var content = HttpSendContent.JsonContent(model, Encoding.UTF8);

            return await httpClient.PostAsync(urlArguments, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, UrlArguments urlArguments, HttpContent content, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlArguments.Complete().Url;

            return await httpClient.PostAsync(requestUrl, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PutAsync<TModel>(this HttpClient httpClient, UrlArguments urlArguments, TModel model, CancellationToken cancellationToken = default)
        {
            var content = HttpSendContent.JsonContent(model, Encoding.UTF8);

            return await httpClient.PutAsync(urlArguments, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, UrlArguments urlArguments, HttpContent content, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlArguments.Complete().Url;

            return await httpClient.PutAsync(requestUrl, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PatchAsync<TModel>(this HttpClient httpClient, UrlArguments urlArguments, TModel model, CancellationToken cancellationToken = default)
        {
            var content = HttpSendContent.JsonContent(model, Encoding.UTF8);

            return await httpClient.PatchAsync(urlArguments, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, UrlArguments urlArguments, HttpContent content, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlArguments.Complete().Url;

            return await httpClient.PatchAsync(requestUrl, content, cancellationToken);
        }

        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, UrlArguments urlArguments, CancellationToken cancellationToken = default)
        {
            string requestUrl = urlArguments.Complete().Url;

            return await httpClient.DeleteAsync(requestUrl, cancellationToken);
        }
    }
}
