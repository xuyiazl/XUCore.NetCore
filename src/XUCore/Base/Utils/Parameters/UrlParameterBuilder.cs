using XUCore.Helpers;
using XUCore.Parameters.Formats;
using System;
using System.Collections.Generic;
using System.Web;

namespace XUCore.Parameters
{
    /// <summary>
    /// Url参数生成器
    /// </summary>
    public class UrlParameterBuilder
    {
        #region 属性

        /// <summary>
        /// 参数生成器
        /// </summary>
        private ParameterBuilder ParameterBuilder { get; }

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        public object this[string name]
        {
            get => GetValue(name);
            set => Add(name, value);
        }

        #endregion 属性

        #region 构造函数

        /// <summary>
        /// 初始化一个<see cref="UrlParameterBuilder"/>类型的实例
        /// </summary>
        public UrlParameterBuilder() : this("") { }

        /// <summary>
        /// 初始化一个<see cref="UrlParameterBuilder"/>类型的实例
        /// </summary>
        /// <param name="builder">参数生成器</param>
        public UrlParameterBuilder(ParameterBuilder builder)
        {
            ParameterBuilder = builder == null ? new ParameterBuilder() : new ParameterBuilder(builder);
        }

        /// <summary>
        /// 初始化一个<see cref="UrlParameterBuilder"/>类型的实例
        /// </summary>
        /// <param name="builder">Url参数生成器</param>
        public UrlParameterBuilder(UrlParameterBuilder builder) : this("", builder) { }

        /// <summary>
        /// 初始化一个<see cref="UrlParameterBuilder"/>类型的实例
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="builder">Url参数生成器</param>
        public UrlParameterBuilder(string url, UrlParameterBuilder builder = null)
        {
            ParameterBuilder =
                builder == null ? new ParameterBuilder() : new ParameterBuilder(builder.ParameterBuilder);
            LoadUrl(url);
        }

        #endregion 构造函数

        #region LoadUrl(加载Url)

        /// <summary>
        /// 加载Url
        /// </summary>
        /// <param name="url">url</param>
        public void LoadUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }

            if (url.Contains("?"))
            {
                url = url.Substring(url.IndexOf("?", StringComparison.Ordinal) + 1);
            }

            var parameters = HttpUtility.ParseQueryString(url);
            foreach (var key in parameters.AllKeys)
            {
                Add(key, parameters.Get(key));
            }
        }

        #endregion LoadUrl(加载Url)

        #region LoadForm(从Request加载表单参数)

        /// <summary>
        /// 从Request加载表单参数
        /// </summary>
        public void LoadForm()
        {
            var form = Web.Request?.Form;
            if (form == null)
            {
                return;
            }

            foreach (var key in form.Keys)
            {
                if (form.ContainsKey(key))
                {
                    Add(key, form[key]);
                }
            }
        }

        #endregion LoadForm(从Request加载表单参数)

        #region LoadQuery(从Request加载查询参数)

        /// <summary>
        /// 从Request加载查询参数
        /// </summary>
        public void LoadQuery()
        {
            var query = Web.Request?.Query;
            if (query == null)
            {
                return;
            }

            foreach (var key in query.Keys)
            {
                if (query.ContainsKey(key))
                {
                    Add(key, query[key]);
                }
            }
        }

        #endregion LoadQuery(从Request加载查询参数)

        #region Add(添加参数)

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public UrlParameterBuilder Add(string key, object value)
        {
            ParameterBuilder.Add(key, value);
            return this;
        }

        #endregion Add(添加参数)

        #region GetValue(获取值)

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns></returns>
        public object GetValue(string name)
        {
            return ParameterBuilder.GetValue(name);
        }

        #endregion GetValue(获取值)

        #region GetDictionary(获取字典)

        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="isSort">是否按参数名排序</param>
        /// <param name="isUrlEncode">是否Url编码</param>
        /// <param name="encoding">字符编码，默认值：UTF-8</param>
        /// <returns></returns>
        public IDictionary<string, object> GetDictionary(bool isSort = true, bool isUrlEncode = false,
            string encoding = "UTF-8")
        {
            return ParameterBuilder.GetDictionary(isSort, isUrlEncode, encoding);
        }

        #endregion GetDictionary(获取字典)

        #region GetKeyValuePairs(获取键值对集合)

        /// <summary>
        /// 获取键值对集合
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> GetKeyValuePairs()
        {
            return ParameterBuilder.GetKeyValuePairs();
        }

        #endregion GetKeyValuePairs(获取键值对集合)

        #region Result(获取结果)

        /// <summary>
        /// 获取结果，格式：参数名=参数值&amp;参数名=参数值
        /// </summary>
        /// <param name="isSort">是否按参数名排序</param>
        /// <param name="isUrlEncode">是否Url编码</param>
        /// <param name="encoding">字符编码，默认值：UTF-8</param>
        /// <returns></returns>
        public string Result(bool isSort = false, bool isUrlEncode = false, string encoding = "UTF-8")
        {
            return ParameterBuilder.Result(UrlParameterFormat.Instance, isSort, isUrlEncode, encoding);
        }

        #endregion Result(获取结果)

        #region JoinUrl(连接Url)

        /// <summary>
        /// 连接Url
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public string JoinUrl(string url)
        {
            return Url.Join(url, Result());
        }

        #endregion JoinUrl(连接Url)

        #region Clear(清空)

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            ParameterBuilder.Clear();
        }

        #endregion Clear(清空)

        #region Remove(移除参数)

        /// <summary>
        /// 移除参数
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return ParameterBuilder.Remove(key);
        }

        #endregion Remove(移除参数)
    }
}