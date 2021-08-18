using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;


namespace XUCore.Template.Layer.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 密码修改命令
    /// </summary>
    public class AdminUserUpdatePasswordCommand : CommandId<bool, long>
    {
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

        public class Validator : CommandIdValidator<AdminUserUpdatePasswordCommand, bool, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(30).WithName("旧密码");
                RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(30).WithName("新密码").NotEqual(c => c.OldPassword).WithName("新密码不能和旧密码相同");
            }
        }
    }
}
