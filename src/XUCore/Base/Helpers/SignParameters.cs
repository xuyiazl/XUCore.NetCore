using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XUCore.Serializer;

namespace XUCore.Helpers
{
    /// <summary>
    /// 签名工具类
    /// </summary>
    public class SignParameters
    {
        private IDictionary<string, string> parameters;
        public SignParameters()
        {
            parameters = new Dictionary<string, string>();
        }

        public static SignParameters Create()
        {
            return new SignParameters();
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public SignParameters Add(string name, string value)
        {
            if (string.IsNullOrEmpty(name)) return this;

            if (!parameters.ContainsKey(name))
                parameters.Add(name, value);

            return this;
        }
        /// <summary>
        /// 获取 Sign 签名
        /// </summary>
        /// <param name="key">key 秘钥的字符名称 就是叫 key</param>
        /// <param name="value">秘钥</param>
        /// <returns></returns>
        public string CreateSign(string key, string value)
        {
            var val = CreateToken(key, value);

            return Encrypt.Md5By32(val).ToLower();
        }

        /// <summary>
        /// 构造验证字符串
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string CreateToken(string key, string value)
        {
            var sb = new StringBuilder();

            var akeys = parameters.Keys.ToList().ToArray();

            Array.Sort(akeys, String.CompareOrdinal); //ASCII排序

            var keys = new List<string> { "sign", "key" };

            foreach (string k in akeys)
            {
                var v = parameters[k];
                if (!string.IsNullOrEmpty(v) && !keys.Contains(k))
                    sb.Append($"{k}={v}&");
            }

            sb.Append(key + "=" + value);

            return sb.ToString();
        }
    }
}
