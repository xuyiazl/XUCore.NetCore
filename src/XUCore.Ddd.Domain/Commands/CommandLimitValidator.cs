using FluentValidation;

namespace XUCore.Ddd.Domain.Commands
{
    /// <summary>
    /// 命令验证抽象基类
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class CommandLimitValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : CommandLimit<TResponse>
    {
        /// <summary>
        /// 注册分页模型验证
        /// </summary>
        /// <param name="max">最大数值（默认100）</param>
        public void AddLimitVaildator(int max = 100)
        {
            RuleFor(x => x.Limit).NotEmpty().GreaterThanOrEqualTo(0).LessThanOrEqualTo(max).WithName("limit");
        }
    }
}
