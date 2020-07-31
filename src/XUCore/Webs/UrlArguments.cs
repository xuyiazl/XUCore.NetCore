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
    using System.Text;

    /// <summary>
    /// Url构造器
    /// </summary>
    public class UrlArguments
    {
        private IDictionary<string, object>
                _parameters = new SortedDictionary<string, object>();

        private string _host;
        public string ClientName { get; set; }
        public string Url { get; private set; }

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
        /// 参数字符串
        /// </summary>
        public string ParameterString
        {
            get
            {
                return _parameters.Select(m => m.Key + "=" + m.Value).DefaultIfEmpty().Aggregate((m, n) => m + "&" + n);
            }
        }

        public UrlArguments()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestUrl">请求url</param>
        public UrlArguments(string requestUrl)
        {
            _host = requestUrl;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientName">请求名</param>
        /// <param name="requestUrl">请求地址</param>
        public UrlArguments(string clientName, string requestUrl)
        {
            _host = requestUrl;
            ClientName = clientName;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="keyValues">请求参数</param>
        public UrlArguments(string requestUrl, params KeyValuePair<string, object>[] keyValues)
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
        public UrlArguments(string clientName, string requestUrl, params KeyValuePair<string, object>[] keyValues)
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
        public static UrlArguments Create() => new UrlArguments();
        /// <summary>
        /// 创建一个新的构造器
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static UrlArguments Create(string requestUrl) => Create(string.Empty, requestUrl);
        /// <summary>
        /// 创建一个新的构造器
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public static UrlArguments Create(string clientName, string requestUrl) => new UrlArguments(clientName, requestUrl);
        /// <summary>
        /// 创建一个新的构造器
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public static UrlArguments Create(string requestUrl, params KeyValuePair<string, object>[] keyValues) => Create(string.Empty, requestUrl, keyValues);
        /// <summary>
        /// 创建一个新的构造器
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="requestUrl"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public static UrlArguments Create(string clientName, string requestUrl, params KeyValuePair<string, object>[] keyValues) => new UrlArguments(clientName, requestUrl, keyValues);
        /// <summary>
        /// 创建一个新的构造器
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="requestUrl"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static UrlArguments Create(string clientName, string requestUrl, string method) => new UrlArguments(clientName, $"{requestUrl}/{method}");
        /// <summary>
        /// 创建一个新的构造器
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="host"></param>
        /// <param name="area"></param>
        /// <param name="controller"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static UrlArguments Create(string clientName, string host, string area = "api", string controller = null, string method = null)
        {
            if (!string.IsNullOrEmpty(method))
            {
                if (!string.IsNullOrEmpty(area))
                {
                    return new UrlArguments(clientName, $"{host}/{area}/{controller}/{method}");
                }
                else
                {
                    return new UrlArguments(clientName, $"{host}/{controller}/{method}");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(area))
                {
                    return new UrlArguments(clientName, $"{host}/{area}/{controller}");
                }
                else
                {
                    return new UrlArguments(clientName, $"{host}/{controller}");
                }
            }
        }
        /// <summary>
        /// 写入请求名
        /// </summary>
        /// <param name="clientName"></param>
        /// <returns></returns>
        public UrlArguments SetClientName(string clientName)
        {
            ClientName = clientName;
            return this;
        }
        /// <summary>
        /// 写入Host
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public UrlArguments SetHost(string host)
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
        public UrlArguments Add(string key, object value)
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
        /// <param name="filter"></param>
        /// <returns></returns>
        public UrlArguments Add(string key, object value, Func<bool> filter)
        {
            if (!filter())
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
        public UrlArguments Remove(string key)
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
        public UrlArguments Clear()
        {
            _parameters.Clear();
            _host = string.Empty;
            return this;
        }
        /// <summary>
        /// 完成构造，生成最终请求Url
        /// </summary>
        /// <returns></returns>
        public UrlArguments Complete()
        {
            StringBuilder url = new StringBuilder();
            url.Append(_host);

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