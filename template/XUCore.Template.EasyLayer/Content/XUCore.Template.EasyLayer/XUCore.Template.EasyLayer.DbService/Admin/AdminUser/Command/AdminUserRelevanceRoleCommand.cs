using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.NetCore.Data;

namespace XUCore.Template.EasyLayer.DbService.Admin.AdminUser
{
    /// <summary>
    /// 关联角色命令
    /// </summary>
    public class AdminUserRelevanceRoleCommand : CreateCommand
    {
        /// <summary>
        /// 管理员id
        /// </summary>
        [Required]
        public long AdminId { get; set; }
        /// <summary>
        /// 角色id集合
        /// </summary>
        public long[] RoleIds { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandValidator<AdminUserRelevanceRoleCommand>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
            }
        }
    }
}
