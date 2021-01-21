using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using XUCore.Extensions;

namespace XUCore.Serializer
{
    /// <summary>
    /// 匹配模式
    /// </summary>
    public enum LimitPropsMode
    {
        /// <summary>
        /// 需要保留的字段
        /// </summary>
        Contains,
        /// <summary>
        /// 要排除的字段
        /// </summary>
        Ignore
    }
    public class LimitPropsCamelCaseContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// 指定要序列化属性的清单
        /// </summary>
        public string[] Props { get; set; }
        /// <summary>
        /// 匹配模式
        /// </summary>
        public LimitPropsMode LimitMode { get; set; }
        /// <summary>
        /// 重命名对照表
        /// </summary>
        public IDictionary<string, string> PropsRename { get; set; }

        public LimitPropsCamelCaseContractResolver()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="props">传入的属性数组</param>
        /// <param name="limitMode">Contains:表示props是需要保留的字段  Ignore：表示props是要排除的字段</param>
        /// <param name="propsRename">重命名对照表</param>
        public LimitPropsCamelCaseContractResolver(string[] props, LimitPropsMode limitMode = LimitPropsMode.Contains, IDictionary<string, string> propsRename = null)
        {
            this.Props = props;
            this.LimitMode = limitMode;
            this.PropsRename = propsRename;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            if (PropsRename == null || PropsRename.Count == 0)
                return base.ResolvePropertyName(propertyName.ToCamelCase());

            if (PropsRename.TryGetValue(propertyName.ToLower(), out string newPropertyName))
                return newPropertyName;
            else
                return base.ResolvePropertyName(propertyName.ToCamelCase());
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

            if (Props == null || Props.Length == 0) return list;

            return list.Where(p =>
            {
                if (LimitMode == LimitPropsMode.Contains)
                    return Props.Any(a => a.ToLower() == p.PropertyName.ToLower());
                else
                    return !Props.Any(a => a.ToLower() == p.PropertyName.ToLower());
            }).ToList();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty prop = base.CreateProperty(member, memberSerialization);

            if (!member.CustomAttributes.Any(att => att.AttributeType == typeof(JsonPropertyAttribute)))
            {
                prop.PropertyName = prop.PropertyName?.ToCamelCase();
            }

            return prop;
        }
    }
}
