using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public abstract class CommandPage<TResponse> : Command<TResponse>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [Required]
        public int CurrentPage { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        [Required]
        public int PageSize { get; set; }
        /// <summary>
        /// 抽象命令基类
        /// </summary>
        protected CommandPage() : base()
        {
        }
    }

    /// <summary>
    /// 命令验证抽象基类
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class CommandPageValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : CommandPage<TResponse>
    {
        /// <summary>
        /// 分页验证(当前页码必须大于0，分页大小必须大于0且小于等于指定大小)
        /// </summary>
        /// <param name="maxPageSize">最大分页大小（默认100）</param>
        public void AddPageVaildator(int maxPageSize = 100)
        {
            RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
            RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(maxPageSize).WithName("PageSize");
        }
    }
}
