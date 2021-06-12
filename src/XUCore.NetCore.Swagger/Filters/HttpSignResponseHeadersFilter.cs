using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using XUCore.NetCore.Signature;

namespace XUCore.NetCore.Swagger
{
    /// <summary>
    /// http签名设置
    /// </summary>
    public class HttpSignResponseHeadersFilter : IOperationFilter
    {
        private readonly string prefix;
        public HttpSignResponseHeadersFilter(string prefix)
        {
            this.prefix = prefix;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            if (context.ApiDescription.TryGetMethodInfo(out MethodInfo methodInfo))
            {
                if (methodInfo.CustomAttributes.Any(t => t.AttributeType.BaseType == typeof(HttpSignAttribute)))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        In = ParameterLocation.Header,
                        Name = $"{prefix}appid",
                        Description = "应用Id",
                        Schema = new OpenApiSchema { Type = "String" },
                    });
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Description = "签名字符串",
                        In = ParameterLocation.Header,
                        Name = $"{prefix}sign",
                        Schema = new OpenApiSchema { Type = "String" },
                    });
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Description = "10位时间戳",
                        In = ParameterLocation.Header,
                        Name = $"{prefix}timestamp",
                        Schema = new OpenApiSchema { Type = "Number" },
                    });
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Description = "16位随机码",
                        In = ParameterLocation.Header,
                        Name = $"{prefix}noncestr",
                        Schema = new OpenApiSchema { Type = "String" },
                    });
                }
            }
        }
    }
}
