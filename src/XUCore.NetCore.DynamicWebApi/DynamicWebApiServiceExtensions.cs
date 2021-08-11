using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.DynamicWebApi.Helper;

namespace XUCore.NetCore.DynamicWebApi
{
    /// <summary>
    /// Add Dynamic WebApi
    /// </summary>
    public static class DynamicWebApiServiceExtensions
    {
        /// <summary>
        /// Add Dynamic WebApi to Container
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options">configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services, DynamicWebApiOptions options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            options.Valid();

            AppConsts.IsRemoveVerbs = options.IsRemoveVerbs;
            AppConsts.DefaultAreaName = options.DefaultAreaName;
            AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
            AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;
            AppConsts.GetRestFulActionName = options.GetRestFulActionName;
            AppConsts.AssemblyDynamicWebApiOptions = options.AssemblyDynamicWebApiOptions;
            AppConsts.SplitActionCamelCase = options.SplitActionCamelCase;
            AppConsts.SplitActionCamelCaseSeparator = options.SplitActionCamelCaseSeparator;
            AppConsts.SplitControllerCamelCase = options.SplitControllerCamelCase;
            AppConsts.SplitControllerCamelCaseSeparator = options.SplitControllerCamelCaseSeparator;
            AppConsts.VersionSeparator = options.VersionSeparator;

            var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddDynamicWebApi\" must be after \"AddMvc\".");
            }

            // Add a custom controller checker
            partManager.FeatureProviders.Add(new DynamicWebApiControllerFeatureProvider());

            services.Configure<MvcOptions>(o =>
            {
                // Register Controller Routing Information Converter
                o.Conventions.Add(new DynamicWebApiConvention());
            });

            return services;
        }

        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services)
        {
            return AddDynamicWebApi(services, new DynamicWebApiOptions());
        }

        public static IServiceCollection AddDynamicWebApi(this IServiceCollection services, Action<DynamicWebApiOptions> optionsAction)
        {
            var dynamicWebApiOptions = new DynamicWebApiOptions();

            optionsAction?.Invoke(dynamicWebApiOptions);

            return AddDynamicWebApi(services, dynamicWebApiOptions);
        }

    }
}
