using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.NetCore.Data;

namespace XUCore.Template.FreeSql.DbService.User.User
{
    /// <summary>
    /// 关联角色命令
    /// </summary>
    public class UserRelevanceRoleCommand : CreateCommand
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public long UserId { get; set; }
        /// <summary>
        /// 角色id集合
        /// </summary>
        public long[] RoleIds { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandValidator<UserRelevanceRoleCommand>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().GreaterThan(0).WithName("UserId");
            }
        }
    }
}
