using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;


namespace Sample2.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 密码修改命令
    /// </summary>
    public class AdminUserUpdatePasswordCommand : Command<bool>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required]
        public long Id { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        public string NewPassword { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandValidator<AdminUserUpdatePasswordCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
                RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(50).WithName("旧密码");
                RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(50).When(c => c.OldPassword != c.NewPassword).WithName("新密码");
            }
        }
    }
}
