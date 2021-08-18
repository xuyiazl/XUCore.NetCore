using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 查询命令
    /// </summary>
    public class AdminUserLoginRecordQueryPagedCommand : CommandPage<bool>
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 排序方式 exp：“Id asc or Id desc”
        /// </summary>
        public string Orderby { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandPageValidator<AdminUserLoginRecordQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}
