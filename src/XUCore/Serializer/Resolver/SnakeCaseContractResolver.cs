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
    /// 驼峰处理，如果使用CamelCasePropertyNamesContractResolver的话，JsonProperty指定大写失效
    /// </summary>
    public class SnakeCaseContractResolver : DefaultContractResolver
    {
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
