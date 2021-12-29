using System;
using System.Collections.Generic;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using XUCore.NetCore.DynamicWebApi;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 命令Api基类
    /// </summary>
    [DynamicWebApi]
    public abstract class CommandApi<TResponse> : Command<TResponse>, IDynamicWebApi
    {
        protected readonly IMediator mediator;

        protected CommandApi(IServiceProvider serviceProvider) : base()
        {
            this.mediator = serviceProvider.GetRequiredService<IMediator>();
        }
    }
}
