using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class CommandLimit<TResponse> : Command<TResponse>
    {
        /// <summary>
        /// 记录数
        /// </summary>
        [Required]
        public int Limit { get; set; }
        /// <summary>
        /// 抽象命令基类
        /// </summary>
        protected CommandLimit() : base()
        {

        }
    }

    /// <summary>
    /// 命令验证抽象基类
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class CommandLimitValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : CommandLimit<TResponse>
    {
        /// <summary>
        /// Limit验证（大于等于0 且小于默认100）
        /// </summary>
        /// <param name="max">最大数值（默认100）</param>
        public void AddLimitVaildator(int max = 100)
        {
            RuleFor(x => x.Limit).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(max).WithName("limit");
        }
    }
}
