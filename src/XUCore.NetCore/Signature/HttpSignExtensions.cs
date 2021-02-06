using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using XUCore.Configs;
using XUCore.Helpers;
using XUCore.NetCore.HttpFactory;
using XUCore.Timing;

namespace XUCore.NetCore.Signature
{
    public static class HttpSignExtensions
    {
        /// <summary>
        /// 注册签名服务，<see cref="HttpSignOptions"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpSignService(this IServiceCollection services, string section = "HttpSignOptions")
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.BindSection<HttpSignOptions>(configuration, section);

            return services;
        }
        /// <summary>
        /// 注册签名中间件，<see cref="HttpSignMiddleware"/>
        /// </summary>
        /// <param name="builder">应用程序生成器</param>
        public static IApplicationBuilder UseHttpSign<TMiddleware>(this IApplicationBuilder builder)
            where TMiddleware : HttpSignMiddleware
        {
            return builder.UseMiddleware<TMiddleware>();
        }
        /// <summary>
        /// 写入签名
        /// </summary>
        /// <param name="client"></param>
        /// <param name="appId">应用id</param>
        /// <param name="appSecret">密钥</param>
        /// <param name="prefix">header 前缀</param>
        public static void SetSignature(this HttpClient client, string appId, string appSecret, string prefix = "x-client-")
        {
            var timestamp = DateTime.Now.ToTimeStamp(false);

            var noncestr = Str.GetNonceStr(16, isCharacter: false);

            var sign =
                SignParameters.Create()
                    .Add("appid", appId)
                    .Add("timestamp", timestamp.ToString())
                    .Add("noncestr", noncestr)
                    .CreateSign("key", appSecret);

            client.SetHeader($"{prefix}appid", appId);
            client.SetHeader($"{prefix}sign", sign);
            client.SetHeader($"{prefix}timestamp", timestamp.ToString());
            client.SetHeader($"{prefix}noncestr", noncestr);
        }
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
}
