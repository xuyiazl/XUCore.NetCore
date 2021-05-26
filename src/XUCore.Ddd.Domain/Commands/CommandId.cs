using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using XUCore.Ddd.Domain.Events;

namespace XUCore.Ddd.Domain.Commands
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class CommandId<TResponse> : Command<TResponse>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 抽象命令基类
        /// </summary>
        protected CommandId() : base()
        {

        }
    }

    /// <summary>
    /// 命令验证抽象基类
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class CommandIdValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : CommandId<TResponse>
    {
        /// <summary>
        /// Id验证（大于0）
        /// </summary>
        public void AddIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("id");
        }
    }
}
