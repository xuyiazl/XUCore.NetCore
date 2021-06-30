using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;


namespace Sample.Plain.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 关联角色命令
    /// </summary>
    public class AdminUserRelevanceRoleCommand : Command<bool>
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

        public class Validator : CommandValidator<AdminUserRelevanceRoleCommand>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
            }
        }
    }
}
