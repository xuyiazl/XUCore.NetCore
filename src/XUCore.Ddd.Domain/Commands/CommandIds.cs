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
    public abstract class CommandIds<TResponse, TKey> : Command<TResponse>
    {
        /// <summary>
        /// 主键Id集合
        /// </summary>
        public TKey[] Ids { get; set; }
        /// <summary>
        /// 抽象命令基类
        /// </summary>
        protected CommandIds() : base()
        {

        }
    }


    /// <summary>
    /// 命令验证抽象基类
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class CommandIdsValidator<TCommand, TResponse, TKey> : AbstractValidator<TCommand>
        where TCommand : CommandIds<TResponse, TKey>
    {
        /// <summary>
        /// Id集合验证（不可为空）
        /// </summary>
        public void AddIdsValidator()
        {
            RuleFor(x => x.Ids).NotEmpty().WithName("ids");
        }
    }
}
