using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XUCore.Extensions;
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
        public static void AddHttpSignDoc(this SwaggerGenOptions options, IServiceCollection services, string prefix = "x-client-")
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
        /// 添加jwt授权认证框
        /// </summary>
        /// <param name="options"></param>
        public static void AddJwtBearerDoc(this SwaggerGenOptions options)
        {
            //添加授权
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });
            //认证方式
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[0]
                    }
                });
        }
        /// <summary>
        /// 添加指定输出文档配置
        /// </summary>
        /// <param name="options"></param>
        public static void AddFiledDoc(this SwaggerGenOptions options)
        {
            options.OperationFilter<FieldResponseHeadersFilter>();
        }
        /// <summary>
        /// 添加文档描述
        /// </summary>
        /// <param name="options"></param>
        /// <param name="type">当前程序集类型，为 Swagger JSON and UI设置xml文档注释路径</param>
        /// <param name="docs">xml注释文档名，请把controller的xml注释文件放第一个</param>
        public static void AddDescriptions(this SwaggerGenOptions options, Type type, params string[] docs)
        {
            // 为 Swagger JSON and UI设置xml文档注释路径
            var basePath = Path.GetDirectoryName(type.Assembly.Location);

            docs.ForEach(xml => options.IncludeXmlComments(Path.Combine(basePath, xml)));

            options.DocumentFilter<ControllerDescriptionFilter>(docs[0]);
        }
        /// <summary>
        /// 添加隐藏API过滤器，可以在API上加HiddenApi标签隐藏。
        /// </summary>
        /// <param name="options"></param>
        /// <param name="apiXml"></param>
        public static void AddHiddenApi(this SwaggerGenOptions options, string apiXml)
        {
            options.DocumentFilter<HiddenApiFilter>();
        }
        /// <summary>
        /// 设置Swagger自动登录
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="accessToken"></param>
        public static void SigninToSwagger(this HttpContext httpContext, string accessToken)
        {
            // 设置 Swagger 刷新自动授权
            httpContext.Response.Headers["access-token"] = accessToken;
        }
        /// <summary>
        /// 设置Swagger自动登录
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="accessToken"></param>
        public static void SigninToSwagger(this IHttpContextAccessor httpContextAccessor, string accessToken)
        {
            httpContextAccessor.HttpContext.SigninToSwagger(accessToken);
        }
        /// <summary>
        /// 设置Swagger退出登录
        /// </summary>
        /// <param name="httpContext"></param>
        public static void SignoutToSwagger(this HttpContext httpContext)
        {
            httpContext.Response.Headers["access-token"] = "invalid_token";
        }
        /// <summary>
        /// 设置Swagger退出登录
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public static void SignoutToSwagger(this IHttpContextAccessor httpContextAccessor)
        {
            httpContextAccessor.HttpContext.SignoutToSwagger();
        }
        /// <summary>
        /// 注入 MiniProfiler 插件（自动登录）
        /// </summary>
        /// <param name="options"></param>
        /// <param name="injectMiniProfiler"></param>
        public static void AddMiniProfiler(this SwaggerUIOptions options, bool injectMiniProfiler = false)
        {
            var thisType = typeof(SwaggerExtensions);
            var thisAssembly = thisType.Assembly;

            // 自定义 Swagger 首页
            options.IndexStream = () => thisAssembly.GetManifestResourceStream($"{thisType.Namespace}.Assets.{(injectMiniProfiler != true ? "index" : "index-mini-profiler")}.html");
        }
    }
}
