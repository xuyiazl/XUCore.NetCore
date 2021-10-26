using System;
using System.Collections.Generic;

using XUCore.NetCore.AccessControl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAccessControl(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            return app.UseMiddleware<AccessControlMiddleware>();
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IAccessControlBuilder AddAccessControl<TResourceAccessStrategy, TControlStrategy>(this IServiceCollection services)
            where TResourceAccessStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IResourceAccessStrategy, TResourceAccessStrategy>();
            services.TryAddSingleton<IControlAccessStrategy, TControlStrategy>();

            return services.AddAccessControl();
        }

        public static IAccessControlBuilder AddAccessControl<TResourceAccessStrategy, TControlStrategy>(this IServiceCollection services, ServiceLifetime resourceAccessStrategyLifetime, ServiceLifetime controlAccessStrategyLifetime)
            where TResourceAccessStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAdd(new ServiceDescriptor(typeof(IResourceAccessStrategy), typeof(TResourceAccessStrategy), resourceAccessStrategyLifetime));
            services.TryAdd(new ServiceDescriptor(typeof(IControlAccessStrategy), typeof(TControlStrategy), controlAccessStrategyLifetime));

            return services.AddAccessControl();
        }

        public static IAccessControlBuilder AddAccessControl<TResourceAccessStrategy, TControlStrategy>(this IServiceCollection services, Action<AccessControlOptions> configAction)
            where TResourceAccessStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configAction != null)
            {
                services.Configure(configAction);
            }
            return services.AddAccessControl<TResourceAccessStrategy, TControlStrategy>();
        }

        public static IAccessControlBuilder AddAccessControl<TResourceAccessStrategy, TControlStrategy>(this IServiceCollection services, Action<AccessControlOptions> configAction, ServiceLifetime resourceAccessStrategyLifetime, ServiceLifetime controlAccessStrategyLifetime)
            where TResourceAccessStrategy : class, IResourceAccessStrategy
            where TControlStrategy : class, IControlAccessStrategy
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configAction != null)
            {
                services.Configure(configAction);
            }
            return services.AddAccessControl<TResourceAccessStrategy, TControlStrategy>(resourceAccessStrategyLifetime, controlAccessStrategyLifetime);
        }

        public static IAccessControlBuilder AddAccessControl(this IServiceCollection services, bool useAsDefaultPolicy)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (useAsDefaultPolicy)
            {
                services.AddAuthorizationCore(options =>
                {
                    var accessControlPolicy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new AccessControlRequirement())
                        .Build();
                    options.AddPolicy(AccessControlConstants.PolicyName, accessControlPolicy);
                    options.DefaultPolicy = accessControlPolicy;
                });
            }
            else
            {
                services.AddAuthorizationCore(options =>
                {
                    var accessControlPolicy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new AccessControlRequirement())
                        .Build();
                    options.AddPolicy(AccessControlConstants.PolicyName, accessControlPolicy);
                });
            }

            services.AddSingleton<IAuthorizationHandler, AccessControlAuthorizationHandler>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return new AccessControlBuilder(services);
        }

        public static IAccessControlBuilder AddAccessControl(this IServiceCollection services)
        {
            return AddAccessControl(services, false);
        }

        public static IAccessControlBuilder AddAccessControl(this IServiceCollection services, Action<AccessControlOptions> configAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var useAsDefaultPolicy = false;
            if (configAction != null)
            {
                var option = new AccessControlOptions();
                configAction.Invoke(option);
                useAsDefaultPolicy = option.UseAsDefaultPolicy;

                services.Configure(configAction);
            }
            return services.AddAccessControl(useAsDefaultPolicy);
        }

        public static IAccessControlBuilder AddResourceAccessStrategy<TResourceAccessStrategy>(this IAccessControlBuilder builder) where TResourceAccessStrategy : IResourceAccessStrategy
        {
            return AddResourceAccessStrategy<TResourceAccessStrategy>(builder, ServiceLifetime.Singleton);
        }

        public static IAccessControlBuilder AddResourceAccessStrategy<TResourceAccessStrategy>(this IAccessControlBuilder builder, ServiceLifetime serviceLifetime) where TResourceAccessStrategy : IResourceAccessStrategy
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Add(
                new ServiceDescriptor(typeof(IResourceAccessStrategy), typeof(TResourceAccessStrategy), serviceLifetime));
            return builder;
        }

        public static IAccessControlBuilder AddControlAccessStrategy<TControlAccessStrategy>(this IAccessControlBuilder builder) where TControlAccessStrategy : IControlAccessStrategy
        {
            return AddControlAccessStrategy<TControlAccessStrategy>(builder, ServiceLifetime.Singleton);
        }

        public static IAccessControlBuilder AddControlAccessStrategy<TControlAccessStrategy>(this IAccessControlBuilder builder, ServiceLifetime serviceLifetime) where TControlAccessStrategy : IControlAccessStrategy
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Add(new ServiceDescriptor(typeof(IControlAccessStrategy), typeof(TControlAccessStrategy), serviceLifetime));
            return builder;
        }
    }
}
