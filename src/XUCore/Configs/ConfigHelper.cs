using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.IO;

namespace XUCore.Configs
{
    /// <summary>
    /// 配置文件 操作辅助类
    /// </summary>
    public static class ConfigHelper
    {
        #region GetJsonConfig(获取Json配置文件)

        /// <summary>
        /// 获取Json配置文件
        /// </summary>
        /// <param name="configFileName">配置文件名。默认：appsettings.json</param>
        /// <param name="basePath">基路径</param>
        /// <returns></returns>
        public static IConfigurationRoot GetJsonConfig(string configFileName = "appsettings.json", string basePath = "")
        {
            basePath = string.IsNullOrWhiteSpace(basePath)
                ? Directory.GetCurrentDirectory()
                : Path.Combine(Directory.GetCurrentDirectory(), basePath);

            var configuration = new ConfigurationBuilder().SetBasePath(basePath)
                .AddJsonFile(configFileName, false, true)
                .Build();

            return configuration;
        }


        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="section"></param>
        public static TOptions GetSection<TOptions>(this IConfiguration configuration, string section) where TOptions : class, new()
        {
            if (configuration != null && !string.IsNullOrEmpty(section))
            {
                try
                {
                    TOptions options = new TOptions();
                    //需要引用Microsoft.Extensions.Configuration.Binder 组件
                    configuration.GetSection(section).Bind(options);
                    return options;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }


        /// <summary>
        /// 绑定获取配置
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration"></param>
        /// <param name="section"></param>
        public static TOptions BindSection<TOptions>(this IServiceCollection services, IConfiguration configuration, string section) where TOptions : class, new()
        {
            if (configuration != null && !string.IsNullOrEmpty(section))
            {
                try
                {
                    //需要引用Microsoft.Extensions.Configuration.Binder 组件
                    var appSection = configuration.GetSection(section);
                    services.Configure<TOptions>(option => appSection.Bind(option));
                    services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TOptions>>().Value);

                    return appSection.Get<TOptions>();
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        #endregion GetJsonConfig(获取Json配置文件)

        #region GetXmlConfig(获取Xml配置文件)

        /// <summary>
        /// 获取Xml配置文件
        /// </summary>
        /// <param name="configFileName">配置文件名。默认：appsettings.xml</param>
        /// <param name="basePath">基路径</param>
        /// <returns></returns>
        public static IConfigurationRoot GetXmlConfig(string configFileName = "appsettings.xml", string basePath = "")
        {
            basePath = string.IsNullOrWhiteSpace(basePath)
                ? Directory.GetCurrentDirectory()
                : Path.Combine(Directory.GetCurrentDirectory(), basePath);

            var configuration = new ConfigurationBuilder().AddXmlFile(config =>
              {
                  config.Path = configFileName;
                  config.FileProvider = new PhysicalFileProvider(basePath);
              });

            return configuration.Build();
        }

        #endregion GetXmlConfig(获取Xml配置文件)
    }
}