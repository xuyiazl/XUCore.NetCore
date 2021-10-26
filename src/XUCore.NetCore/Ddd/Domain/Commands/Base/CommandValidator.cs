using FluentValidation;

namespace XUCore.Ddd.Domain.Commands
{
    /// <summary>
    /// 命令验证抽象基类
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
    {
    }
}
