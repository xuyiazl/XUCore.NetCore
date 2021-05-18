using FluentValidation;

namespace XUCore.Ddd.Domain.Commands
{
    /// <summary>
    /// 命令验证抽象基类
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class CommandIdsValidator<TCommand, TResponse> : AbstractValidator<TCommand>
        where TCommand : CommandIds<TResponse>
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
