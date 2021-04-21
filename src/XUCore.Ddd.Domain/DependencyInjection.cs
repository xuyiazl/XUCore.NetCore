using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Behaviours;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Events;

namespace XUCore.Ddd.Domain
{
    public static class DependencyInjection
    {
        /// <summary>
        /// 注册 DDD Mediator 性能监控
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPerformanceBehaviour(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestLogger<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));

            return services;
        }
        /// <summary>
        /// 注册 DDD 自动模型验证
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidationBehavior(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
        /// <summary>
        /// 注册 DDD Mediator 消息总线
        /// </summary>
        /// <typeparam name="TMediator"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediatorBus<TMediator>(this IServiceCollection services)
            where TMediator : class, IMediatorHandler
        {
            services.AddScoped<IMediatorHandler, TMediator>();

            return services;
        }
        /// <summary>
        /// 注册 DDD 事件存储
        /// </summary>
        /// <typeparam name="TEventStore"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventStore<TEventStore>(this IServiceCollection services)
            where TEventStore : class, IEventStoreService
        {
            services.AddScoped<IEventStoreService, TEventStore>();

            return services;
        }
    }
}
