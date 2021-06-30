using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace Sample.Plain.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 登录命令
    /// </summary>
    public class AdminUserLoginCommand
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [Required]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        public class Validator : CommandValidator<AdminUserLoginCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Account).NotEmpty().WithName("登录账号");
                RuleFor(x => x.Password).NotEmpty().WithName("登录密码");
            }
        }
    }
}
