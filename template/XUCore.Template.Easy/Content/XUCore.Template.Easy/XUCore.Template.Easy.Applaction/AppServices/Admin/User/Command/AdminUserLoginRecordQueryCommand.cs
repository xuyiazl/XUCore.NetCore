using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.NetCore.Data;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 查询命令
    /// </summary>
    public class AdminUserLoginRecordQueryCommand : ListCommand
    {
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
