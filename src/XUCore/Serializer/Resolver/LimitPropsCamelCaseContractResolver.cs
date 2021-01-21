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
            string newPropertyName = string.Empty;
            if (PropsRename != null && PropsRename.TryGetValue(propertyName.ToLower(), out newPropertyName))
            {
                return newPropertyName;
            }
            else
            {
                //没有找到就用原来的
                return base.ResolvePropertyName(propertyName);
            }
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

            if (Props == null) return list;

            //只保留清单有列出的属性
            return list.Where(p =>
            {
                if (LimitMode == LimitPropsMode.Contains)
                {
                    //匹配的时候为了不区分大小写
                    return Props.FirstOrDefault(a => a.ToLower() == p.PropertyName.ToLower()) != null;
                }
                else
                {
                    return Props.FirstOrDefault(a => a.ToLower() == p.PropertyName.ToLower()) == null;
                }
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
