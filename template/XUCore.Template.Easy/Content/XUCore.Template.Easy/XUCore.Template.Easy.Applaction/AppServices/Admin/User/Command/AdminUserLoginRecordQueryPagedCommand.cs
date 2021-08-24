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
    public class AdminUserLoginRecordQueryPagedCommand : PageCommand
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

        public class Validator : CommandPageValidator<AdminUserLoginRecordQueryPagedCommand, bool>
        {
            public Validator()
            {

                RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
                RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("PageSize");
            }
        }
    }
}
