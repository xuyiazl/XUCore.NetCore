using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XUCore.NetCore.Swagger
{
    public class FieldResponseHeadersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            if (context.ApiDescription.TryGetMethodInfo(out MethodInfo methodInfo))
            {
                if (methodInfo.CustomAttributes.Any(t => t.AttributeType == typeof(FieldResponseAttribute)))
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Description = "（选填）输出模式，contain：指定模式，ignore：忽略模式",
                        In = ParameterLocation.Header,
                        Name = "limit-mode",
                        Schema = new OpenApiSchema { Type = "String" },
                    });
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Description = "（选填）指定需要的字段，exp：col1,col2，注意：使用重名后的字段",
                        In = ParameterLocation.Header,
                        Name = "limit-field",
                        Schema = new OpenApiSchema { Type = "String" },
                    });
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Description = "（选填）字段重命名，exp：column1=col1,column2=col2",
                        In = ParameterLocation.Header,
                        Name = "limit-field-rename",
                        Schema = new OpenApiSchema { Type = "String" },
                    });
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Description = "（选填）日期输出格式，exp：yyyy-MM-dd HH:mm:ss",
                        In = ParameterLocation.Header,
                        Name = "limit-date-format",
                        Schema = new OpenApiSchema { Type = "String" },
                    });
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Description = "（选填）指定所有日期以时间戳输出",
                        In = ParameterLocation.Header,
                        Name = "limit-date-unix",
                        Schema = new OpenApiSchema { Type = "Boolean" },
                    });
                }
            }
        }
    }
}
