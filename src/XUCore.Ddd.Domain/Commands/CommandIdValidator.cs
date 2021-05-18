using FluentValidation;

namespace XUCore.Ddd.Domain.Commands
{
    /// <summary>
    /// 命令验证抽象基类
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class CommandIdValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : CommandId<TResponse>
    {
        /// <summary>
        /// 注册分页模型验证
        /// </summary>
        public void AddIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("id");
        }
    }
}
