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
    public class UserLoginRecordQueryPagedCommand : PageCommand
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandPageValidator<UserLoginRecordQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}
