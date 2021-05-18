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
