using XUCore.Extensions;
using XUCore.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace XUCore.NetCore.HttpFactory
{
    public static class HttpHeaderExtensions
    {
        public static HttpClient SetHeader(this HttpClient client, Action<HttpRequestHeaders> action)
        {
            action.Invoke(client.DefaultRequestHeaders);

            return client;
        }

        public static HttpClient SetHeader(this HttpClient client, string name, string value)
        {
            client.DefaultRequestHeaders.Add(name, value);

            return client;
        }

        public static HttpClient SetHeaderAccept(this HttpClient client, HttpMediaType mediaType)
        {
            client.SetHeaderAccept(mediaType.Description());

            return client;
        }

        public static HttpClient SetHeaderAccept(this HttpClient client, string mediaType)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            return client;
        }

        public static HttpClient SetHeaderToken(this HttpClient client, string scheme, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);

            return client;
        }

        public static HttpClient SetHeaderBearerToken(this HttpClient client, string token)
        {
            client.SetHeaderToken("Bearer", token);

            return client;
        }

        public static HttpClient SetHeaderClientIP(this HttpClient client, string clientIP = "")
        {
            client.SetHeader("Client-IP", string.IsNullOrEmpty(clientIP) ? Web.IP : clientIP);

            return client;
        }

        public static HttpRequestMessage SetHeader(this HttpRequestMessage request, Action<HttpRequestHeaders> action)
        {
            action.Invoke(request.Headers);

            return request;
        }

        public static HttpRequestMessage SetHeader(this HttpRequestMessage request, string name, string value)
        {
            request.Headers.Add(name, value);

            return request;
        }

        public static HttpRequestMessage SetHeaderAccept(this HttpRequestMessage request, HttpMediaType mediaType)
        {
            request.SetHeaderAccept(mediaType.Description());

            return request;
        }

        public static HttpRequestMessage SetHeaderAccept(this HttpRequestMessage request, string mediaType)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            return request;
        }

        public static HttpRequestMessage SetHeaderToken(this HttpRequestMessage request, string scheme, string token)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(scheme, token);

            return request;
        }

        public static HttpRequestMessage SetHeaderBearerToken(this HttpRequestMessage request, string token)
        {
            request.SetHeaderToken("Bearer", token);

            return request;
        }

        public static HttpRequestMessage SetHeaderClientIP(this HttpRequestMessage request, string clientIP = "")
        {
            request.SetHeader("Client-IP", string.IsNullOrEmpty(clientIP) ? Web.IP : clientIP);

            return request;
        }


    }
}
