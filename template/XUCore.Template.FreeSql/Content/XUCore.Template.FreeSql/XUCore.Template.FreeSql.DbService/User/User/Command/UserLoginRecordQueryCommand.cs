using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.NetCore.Data;

namespace XUCore.Template.FreeSql.DbService.User.User
{
    /// <summary>
    /// 查询命令
    /// </summary>
    public class UserLoginRecordQueryCommand : ListCommand
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long userId { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandLimitValidator<UserLoginRecordQueryCommand, bool>
        {
            public Validator()
            {
            }
        }
    }
}
