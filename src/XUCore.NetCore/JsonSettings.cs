using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore
{
    /// <summary>
    /// JSON序列化统一配置
    /// </summary>
    public static class JsonSettings
    {
        /// <summary>
        /// UTC + 小驼峰
        /// </summary>
        public static JsonSerializerSettings Utc_CamelCase
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            }
        }
        /// <summary>
        /// 序列化默认配置（随时可以统一修改）
        /// </summary>
        public static JsonSerializerSettings Local_CamelCase
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            }
        }
    }
}
