using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Template.EasyLayer.Core.Enums;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.AdminRole
{
    /// <summary>
    /// 角色查询命令
    /// </summary>
    public class AdminRoleQueryPagedCommand : Command<bool>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [Required]
        public int CurrentPage { get; set; } = 1;
        /// <summary>
        /// 分页大小
        /// </summary>
        [Required]
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 搜索字段
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Search { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 排序方式 exp：“asc or desc”
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandValidator<AdminRoleQueryPagedCommand>
        {
            public Validator()
            {
                RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("页码");
                RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("分页大小");
            }
        }
    }
}
