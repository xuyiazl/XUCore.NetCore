using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.NetCore.Data;

namespace XUCore.Template.Layer.DbService.Admin.AdminUser
{
    /// <summary>
    /// 查询命令
    /// </summary>
    public class AdminUserLoginRecordQueryCommand : ListCommand
    {
        /// <summary>
        /// 管理员id
        /// </summary>
        public long AdminId { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandLimitValidator<AdminUserLoginRecordQueryCommand, bool>
        {
            public Validator()
            {
            }
        }
    }
}
