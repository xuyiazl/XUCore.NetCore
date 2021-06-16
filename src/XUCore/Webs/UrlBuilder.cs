namespace XUCore.Webs
{
    /********************************************************************
    *           Copyright:       2009-2011
    *           Company:
    *           CRL Version :    4.0.30319.239
    *           Created by 徐毅 at 2011/11/29 12:46:54
    *                   mailto:3624091@qq.com
    *
    ********************************************************************/

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using XUCore.Extensions;

    /// <summary>
    /// Url构造器
    /// </summary>
    public class UrlBuilder
    {
        private IDictionary<string, object>
                _parameters = new SortedDictionary<string, object>();

        private string _host;
        public string ClientName { get; set; }
        public string Url { get; private set; }
        private bool _isCompleteParameter = true;

        /// <summary>
        /// 参数列表
        /// </summary>
        public IDictionary<string, object> Parameters
        {
            get
            {
                return _parameters;
            }
        }
        /// <summary>
        /// Form Url Encode
        /// </summary>
        public HttpContent FormUrlEncodedContent
        {
            get
            {
                return new FormUrlEncodedContent(_parameters.Select(m => new KeyValuePair<string?, string?>(m.Key, m.Value.SafeString())));
            }
        }
        /// <summary>
        /// 参数字符串
        /// </summary>
        public string ParameterString
        {
            get
            {
                return _parameters.Select(m => m.Key + "=" + m.Value).DefaultIfEmpty().Aggregate((m, n) => m + "&" + n);
            }
        }

        public UrlBuilder()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestUrl">请求url</param>
        public UrlBuilder(string requestUrl)
        {
            _host = requestUrl;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientName">请求名</param>
        /// <param name="requestUrl">请求地址</param>
        public UrlBuilder(string clientName, string requestUrl)
        {
            _host = requestUrl;
            ClientName = clientName;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="keyValues">请求参数</param>
        public UrlBuilder(string requestUrl, params KeyValuePair<string, object>[] keyValues)
        {
            _host = requestUrl;
            foreach (var item in keyValues)
            {
                this.Add(item.Key, item.Value);
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientName">请求名</param>
        /// <param name="requestUrl">请求参数</param>
        /// <param name="keyValues">请求参数</param>
        public UrlBuilder(string clientName, string requestUrl, params KeyValuePair<string, object>[] keyValues)
        {
            _host = requestUrl;
            ClientName = clientName;
            foreach (var item in keyValues)
            {
                this.Add(item.Key, item.Value);
            }
        }
        /// <summary>
        /// 创建一个新的构造器
        /// </summary>
        /// <returns></returns>
        public static UrlBuilder Create() => new UrlBuilder();
        /// <summary>
        /// 创建一个新的构造器
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static UrlBuilder Create(string requestUrl) => Create(string.Empty, requestUrl);
        /// <summary>
        /// 创建一个新的构造器
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static UrlBuilder Create(string clientName, string requestUrl) => new UrlBuilder(clientName, requestUrl);
        /// <summary>
        /// 是否合并参数
        /// </summary>
        /// <param name="completeParameter">是否合并参数</param>
        /// <returns></returns>
        public UrlBuilder SetCompleteParameter(bool completeParameter)
        {
            _isCompleteParameter = completeParameter;
            return this;
        }
        /// <summary>
        /// 写入请求名
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        public UrlBuilder SetClientName(string clientName)
        {
            ClientName = clientName;
            return this;
        }
        /// <summary>
        /// 写入Host
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public UrlBuilder SetHost(string host)
        {
            _host = host;
            return this;
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public UrlBuilder Add(string key, object value)
        {
            if (!_parameters.ContainsKey(key))
            {
                _parameters.Add(key, value);
            }
            else
            {
                _parameters[key] = value;
            }

            return this;
        }
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public UrlBuilder Add(string key, object value, bool condition)
        {
            if (!condition)
            {
                return this;
            }

            if (!_parameters.ContainsKey(key))
            {
                _parameters.Add(key, value);
            }
            else
            {
                _parameters[key] = value;
            }

            return this;
        }
        /// <summary>
        /// 删除参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public UrlBuilder Remove(string key)
        {
            if (_parameters != null)
            {
                if (_parameters.ContainsKey(key))
                {
                    _parameters.Remove(key);
                }
            }

            return this;
        }
        /// <summary>
        /// 清空Url
        /// </summary>
        /// <returns></returns>
        public UrlBuilder Clear()
        {
            _parameters.Clear();
            _host = string.Empty;
            return this;
        }
        /// <summary>
        /// 完成构造，生成最终请求Url
        /// </summary>
        /// <returns></returns>
        public UrlBuilder Complete()
        {
            StringBuilder url = new StringBuilder();
            url.Append(_host);

            if (_isCompleteParameter)
            {
                if (_parameters.Count == 0)
                {
                    Url = url.ToString();
                    return this;
                }
                if (!_host.EndsWith("&"))
                {
                    url.Append("?");
                }

                url.Append(ParameterString);
            }

            Url = url.ToString();
            return this;
        }
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Complete().Url;
        }
    }
}