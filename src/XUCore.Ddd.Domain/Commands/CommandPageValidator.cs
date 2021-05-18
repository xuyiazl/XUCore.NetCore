using FluentValidation;

namespace XUCore.Ddd.Domain.Commands
{
    /// <summary>
    /// 命令验证抽象基类
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class CommandPageValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : CommandPage<TResponse>
    {
        /// <summary>
        /// 注册分页模型验证
        /// </summary>
        public void AddPageVaildator()
        {
            RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
            RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("PageSize");
        }
    }
}
