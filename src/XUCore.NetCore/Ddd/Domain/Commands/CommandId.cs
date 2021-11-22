using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class CommandId<TResponse, TKey> : Command<TResponse>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required]
        public TKey Id { get; set; }
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
    /// <typeparam name="TKey"></typeparam>
    public abstract class CommandIdValidator<TCommand, TResponse, TKey> : AbstractValidator<TCommand>
        where TCommand : CommandId<TResponse, TKey>
    {
        /// <summary>
        /// Id验证
        /// </summary>
        public void AddIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithName("id");
        }
    }
}
