using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Signature;

namespace XUCore.NetCore.Swagger
{
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 添加签名header
        /// </summary>
        /// <param name="options"></param>
        /// <param name="services"></param>
        /// <param name="prefix"></param>
        public static void SwaggerHttpSignDoc(this SwaggerGenOptions options, IServiceCollection services, string prefix = "x-client-")
        {
            var signOptions = services.BuildServiceProvider().GetService<HttpSignOptions>();
            if (signOptions != null && signOptions.IsOpen)
            {
                prefix = signOptions.Prefix;

                options.AddSecurityDefinition($"{prefix}appid", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = $"{prefix}appid",
                    Description = "应用id",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "string"
                });
                options.AddSecurityDefinition($"{prefix}sign", new OpenApiSecurityScheme
                {
                    Description = "签名字符串",
                    In = ParameterLocation.Header,
                    Name = $"{prefix}sign",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "string"
                });
                options.AddSecurityDefinition($"{prefix}timestamp", new OpenApiSecurityScheme
                {
                    Description = "10位时间戳",
                    In = ParameterLocation.Header,
                    Name = $"{prefix}timestamp",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "long"
                });
                options.AddSecurityDefinition($"{prefix}noncestr", new OpenApiSecurityScheme
                {
                    Description = "16位随机码",
                    In = ParameterLocation.Header,
                    Name = $"{prefix}noncestr",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "string"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = $"{prefix}appid"
                            }
                        },
                        new string[] { }
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = $"{prefix}sign"
                            }
                        },
                        new string[] { }
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = $"{prefix}timestamp"
                            }
                        },
                        new string[] { }
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = $"{prefix}noncestr"
                            }
                        },
                        new string[] { }
                    }
                });
            }

            options.OperationFilter<HttpSignResponseHeadersFilter>(prefix);

        }

        /// <summary>
        /// 添加指定输出
        /// </summary>
        /// <param name="options"></param>
        public static void SwaggerFiledDoc(this SwaggerGenOptions options)
        {
            options.OperationFilter<FieldResponseHeadersFilter>();
        }

        /// <summary>
        /// 添加Controller的日志描述
        /// </summary>
        /// <param name="options"></param>
        /// <param name="apiXml"></param>
        public static void SwaggerControllerDescriptions(this SwaggerGenOptions options, string apiXml)
        {
            options.DocumentFilter<ControllerDescriptions>(apiXml);
            //添加隐藏API过滤器，可以在API上加HiddenApi标签隐藏。
            options.DocumentFilter<HiddenApiFilter>();
        }
    }
}
