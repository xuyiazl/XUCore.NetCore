using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore
{
    /// <summary>
    /// 全局统一序列化配置
    /// </summary>
    public static class GlobalSettings
    {
        /// <summary>
        /// JSON序列化配置（UTC + 小驼峰）
        /// </summary>
        public static JsonSerializerSettings Json_Utc_CamelCase
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
        /// JSON序列化配置（UTC + 大驼峰）
        /// </summary>
        public static JsonSerializerSettings Json_Utc_Default
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    ContractResolver = new DefaultContractResolver()
                };
            }
        }
        /// <summary>
        /// JSON序列化配置（Local + 小驼峰）
        /// </summary>
        public static JsonSerializerSettings Json_Local_CamelCase
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
        /// <summary>
        /// JSON序列化配置（Local + 大驼峰）
        /// </summary>
        public static JsonSerializerSettings Json_Local_Default
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
