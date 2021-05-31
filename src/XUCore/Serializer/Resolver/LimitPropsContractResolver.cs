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

    /// <summary>
    /// 解析器 camelcase 小驼峰 ， default 默认大写
    /// </summary>
    public enum ResolverMode
    {
        /// <summary>
        /// 默认，首字母大写
        /// </summary>
        Default,
        /// <summary>
        /// 小驼峰
        /// </summary>
        CamelCase
    }

    public class LimitPropsContractResolver : DefaultContractResolver
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
        /// <summary>
        /// 是否返回驼峰
        /// </summary>
        public ResolverMode Resolver { get; set; }

        public LimitPropsContractResolver()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="props">传入的属性数组</param>
        /// <param name="limitMode">Contains:表示props是需要保留的字段  Ignore：表示props是要排除的字段</param>
        /// <param name="propsRename">重命名对照表</param>
        /// <param name="resolver">解析器 camelcase 小驼峰 ， default 默认大写</param>
        public LimitPropsContractResolver(string[] props, LimitPropsMode limitMode = LimitPropsMode.Contains, IDictionary<string, string> propsRename = null, ResolverMode resolver = ResolverMode.Default)
        {
            this.Props = props;
            this.LimitMode = limitMode;
            this.PropsRename = propsRename;
            this.Resolver = resolver;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            if (PropsRename == null || PropsRename.Count == 0)
                return base.ResolvePropertyName(ResolverPropertyName(propertyName));

            if (PropsRename.TryGetValue(propertyName.ToLower(), out string newPropertyName))
                return newPropertyName;
            else
                return base.ResolvePropertyName(ResolverPropertyName(propertyName));
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
                prop.PropertyName = ResolverPropertyName(prop.PropertyName);

            return prop;
        }

        private string ResolverPropertyName(string propertyName)
        {
            switch (Resolver)
            {
                case ResolverMode.CamelCase:
                    return propertyName.ToCamelCase();
                default:
                    return propertyName;
            }
        }
    }
}
