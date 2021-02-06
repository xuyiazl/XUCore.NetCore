using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XUCore.NetCore.Signature;

namespace XUCore.NetCore.ApiTests
{
    public class SignDemo : HttpSignMiddleware
    {
        public SignDemo(RequestDelegate next, IOptions<HttpSignOptions> options)
            : base(next, options)
        {

        }

        public override Task<string> GetAppSecretAsync(string appid)
        {
            appid = "web1ed21e4udroo37fmj";

            return Task.FromResult("CdzL5v9s6cmYOqeYW2ZicfdTaT3LdXhJ");
        }
    }

    public class SignActionAttribute : HttpSignAttribute
    {
        public override Task<string> GetAppSecretAsync(IServiceProvider serviceProvider, string appid)
        {
            appid = "web1ed21e4udroo37fmj";

            return Task.FromResult("CdzL5v9s6cmYOqeYW2ZicfdTaT3LdXhJ");
        }
    }

    public static class SwaggerEx
    {
        /// <summary>
        /// 添加swagger 签名header
        /// </summary>
        /// <param name="options"></param>
        /// <param name="services"></param>
        /// <param name="prefix"></param>
        public static void SetHttpSignHeaders(this SwaggerGenOptions options, IServiceCollection services, string prefix = "x-client-")
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
    }


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
